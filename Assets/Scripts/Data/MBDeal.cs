using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
public class MBDeal: MonoBehaviour {
	const byte _t = (byte)'\t';
    const byte _n = (byte)'\n';
	
    static public byte[] readPart(byte[] dat, int start)
    {
        if (start >= dat.Length)
        {
            return null;
        }
        byte[] rst = null;
        int i = start;
        while (i < dat.Length)
        {
            if (dat[i] == _t || dat[i] == _n)
            {
                int l   = i-start+1;
                rst = new byte[l];
               	Array.Copy(dat, start, rst, 0, l);
                return rst;
            }
            i++;
        }
        
        rst = new byte[dat.Length - start];
        Array.Copy(dat, start, rst, 0, rst.Length);
        return rst;
    }
	
    static public string[][] readMBFromBytes(byte[] dat, bool readTitle, bool completeTable)
    {
        if (dat==null)
        {
            return null;
        }
        
        List<string> names = new List<string>();
        int idx = 0;
        bool eol = false;
        byte[] part;
        while (!eol)
        {
            part = readPart(dat, idx);
            if (part == null)
            {
                return null;
            }
		
            if (part[part.Length - 1] != _t)
            {
                eol = true;
            }
            idx += part.Length;
            string s = Encoding.UTF8.GetString(part);
            s   = s.Trim();

            names.Add(s);
        }
        
        int i,j;
		
        List<string>[] columnList = new List<string>[names.Count];
        for (i = 0; i < names.Count; i++)
        {
            columnList[i]   = new List<string>();
        }
  
        int line=0;
        bool eof=false;
        i   = 0;    
        while(!eof)
        {
            List<string>    curList = columnList[i];    
            part = readPart(dat, idx);
            if (part == null)
            {
                if (completeTable)
                {
                    bool needComplete = false;
                    int nMax = columnList[0].Count;
                    for (i = 1; i < names.Count; i++)
                    {
                        if (columnList[i].Count > nMax)
                        {
                            nMax = columnList[i].Count;
                            needComplete = true;
                        }
                        else if( columnList[i].Count != columnList[i-1].Count )
                        {
                            needComplete = true;
                        }
                    }
                    if (needComplete)
                    {
                        for (i = 0; i < names.Count; i++)
                        {
                            curList = columnList[i];
                            while (curList.Count < nMax)
                            {
                                curList.Add(null);
                            }
                        }
                    }
                }
                break;
            }
            idx += part.Length;
            string s = Encoding.UTF8.GetString(part);
            s   = s.Trim();
            if (s.Length > 0)
            {			
				
                while (curList.Count < line)
                {
                    curList.Add(null);
                }
                curList.Add(s);
            }
            if (part[part.Length - 1] != _t)
            {
                i = 0;

                line++;

            }
            else
            {
                i++;
            }
        }

        string[][] rst  = new string[columnList.Length][];
        for (i = 0; i < columnList.Length; i++)
        {
            List<string> sl = columnList[i];
            string[] sa;
       
            if( readTitle )
            {
                sa = new string[sl.Count + 1];
                sa[0] = names[i];
                idx = 1;
            }
            else
            {
                sa = new string[sl.Count];
                idx = 0;
            }
  
            for(j=0;j<sl.Count;j++)
            {
                sa[idx++] = sl[j];
            }
            rst[i] = sa;
        }
        
        return rst;
    }
	
    static public byte[] readFile(string path)
    {
        FileInfo fi = new FileInfo(path);
        using (FileStream fs = fi.OpenRead())
        {
            byte[] dat = new byte[fi.Length];
            if (fs.Read(dat, 0, dat.Length) != dat.Length)
            {
              	Console.WriteLine("data read error");
                return null;
            }
            fs.Close();
            return dat;
        }
    }
}
