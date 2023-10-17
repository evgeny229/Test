using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;


public class ByteBuffer : IDisposable
{
    private List<byte> Buff;
    private byte[] readBuff;
    private int readPos;
    private bool buffIsUpdated = false;
    public ByteBuffer()
    {
        Buff = new List<byte>();
        readPos = 0;
    }
    public int GetReadPosition()
    {
        return readPos;
    }
    public byte[] ToByteArray()
    {
        return Buff.ToArray();
    }
    public int Count()
    {
        return Buff.Count();
    }
    public int Length()
    {
        return Count() - readPos;
    }
    public void Clear()
    {
        Buff.Clear();
        readPos = 0;
    }
    public void WriteByte(byte input)
    {
        Buff.Add(input);
        buffIsUpdated = true;
    }
    public void WriteBytes(byte[] input)
    {
        Buff.AddRange(input);
        buffIsUpdated = true;
    }
    public void WriteShort(short input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffIsUpdated = true;
    }
    public void WriteInteger(int input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffIsUpdated = true;
    }
    public void WriteLong(long input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffIsUpdated = true;
    }
    public void WriteFloat(float input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffIsUpdated = true;
    }
    public void WriteBool(bool input)
    {
        Buff.AddRange(BitConverter.GetBytes(input));
        buffIsUpdated = true;
    }
    public void WriteString(string input)
    {
        Buff.AddRange(BitConverter.GetBytes(input.Length));
        Buff.AddRange(Encoding.ASCII.GetBytes(input));
        buffIsUpdated = true;
    }
    public void WriteIntArray(int[] input)
    {
        Buff.AddRange(BitConverter.GetBytes(input.Length));
        for (int i = 0; i < input.Length; i++)
            Buff.AddRange(BitConverter.GetBytes(input[i]));
        buffIsUpdated = true;
    }
    public int[] ReadIntArray(bool Peek = true)
    {
        try
        {
            int length = ReadInteger(true);
            int[] resultArray = new int[length];
            if (buffIsUpdated)
            {

                readBuff = Buff.ToArray();
                buffIsUpdated = false;
            }
            for (int i = 0; i < length; i++)
            {
                resultArray[i] = BitConverter.ToInt32(readBuff, readPos);

                if (Peek & Buff.Count() > readPos)
                {
                    readPos += 4;
                }
            }
            return resultArray;
        }
        catch (Exception e)
        {
            throw new Exception("you are trying to read none int[] Array");
        }
    }

    public byte ReadByte(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffIsUpdated)
            {
                readBuff = Buff.ToArray();
                buffIsUpdated = false;
            }
            byte value = readBuff[readPos];
            if (Peek && Buff.Count > readPos)
            {
                readPos++;
            }
            return value;
        }
        else
        {
            throw new Exception("you are trying to read none BYTE");
        }
    }
    public byte[] ReadBytes(int length, bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffIsUpdated)
            {
                readBuff = Buff.ToArray();
                buffIsUpdated = false;
            }
            byte[] value = Buff.GetRange(readPos, length).ToArray();
            if (Peek)
            {
                readPos += length;
            }
            return value;
        }
        else
        {
            throw new Exception("you are trying to read none BYTE Array");
        }
    }
    public short ReadShort(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffIsUpdated)
            {
                readBuff = Buff.ToArray();
                buffIsUpdated = false;
            }
            short value = BitConverter.ToInt16(readBuff, readPos);
            if (Peek & Buff.Count() > readPos)
            {
                readPos += 2;
            }
            return value;
        }
        else
        {
            throw new Exception("you are trying to read none Short Array");
        }
    }
    public int ReadInteger(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffIsUpdated)
            {
                readBuff = Buff.ToArray();
                buffIsUpdated = false;
            }
            int value = BitConverter.ToInt32(readBuff, readPos);
            if (Peek & Buff.Count() > readPos)
            {
                readPos += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("you are trying to read none Int Array");
        }
    }
    public long ReadLong(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffIsUpdated)
            {
                readBuff = Buff.ToArray();
                buffIsUpdated = false;
            }
            long value = BitConverter.ToInt64(readBuff, readPos);
            if (Peek & Buff.Count() > readPos)
            {
                readPos += 8;
            }
            return value;
        }
        else
        {
            throw new Exception("you are trying to read none Long Array");
        }
    }
    public float ReadFloat(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffIsUpdated)
            {
                readBuff = Buff.ToArray();
                buffIsUpdated = false;
            }
            float value = BitConverter.ToSingle(readBuff, readPos);
            if (Peek & Buff.Count() > readPos)
            {
                readPos += 4;
            }
            return value;
        }
        else
        {
            throw new Exception("you are trying to read none FLOAT Array");
        }
    }
    public bool ReadBool(bool Peek = true)
    {
        if (Buff.Count > readPos)
        {
            if (buffIsUpdated)
            {
                readBuff = Buff.ToArray();
                buffIsUpdated = false;
            }
            bool value = BitConverter.ToBoolean(readBuff, readPos);
            if (Peek & Buff.Count() > readPos)
            {
                readPos++;
            }
            return value;
        }
        else
        {
            throw new Exception("you are trying to read none BOOL Array");
        }
    }
    public string ReadString(bool Peek = true)
    {
        try
        {
            int length = ReadInteger(true);
            if (buffIsUpdated)
            {
                readBuff = Buff.ToArray();
                buffIsUpdated = false;
            }
            string value = Encoding.ASCII.GetString(readBuff, readPos, length);

            if (Peek & Buff.Count() > readPos)
            {
                if (value.Length > 0)
                {
                    readPos += length;
                }
            }
            return value;
        }
        catch (Exception e)
        {
            throw new Exception("you are trying to read none String Array");
        }
    }
    private bool disposeValue = false;
    protected virtual void Dispose(bool disposing)
    {
        if (!disposing)
        {
            if (disposing)
            {
                Buff.Clear();
                readPos = 0;
            }

            disposeValue = true;
        }
    }
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}