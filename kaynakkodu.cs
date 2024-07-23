using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace systemd
{
    [Serializable]
    [ComVisible(false)]
    public sealed class str
    {
        private char[] characters;

        public str(char[] value)
        {
            characters = new char[value.Length];
            Array.Copy(value, characters, value.Length);
        }

        public str(string value)
        {
            characters = value.ToCharArray();
        }

        public int length
        {
            get { return characters.Length; }
        }

        public char this[int index]
        {
            get
            {
                if (index < 0 || index >= characters.Length)
                {
                    throw new IndexOutOfRangeException();
                }
                return characters[index];
            }
        }

        public str Concat(str s)
        {
            char[] newChars = new char[this.length + s.length];
            Array.Copy(this.characters, 0, newChars, 0, this.length);
            Array.Copy(s.characters, 0, newChars, this.length, s.length);
            return new str(newChars);
        }

        public override string ToString()
        {
            return new string(characters);
        }

        public static implicit operator str(string value)
        {
            return new str(value);
        }

        public static implicit operator string(str myStr)
        {
            return myStr.ToString();
        }

        public static bool operator ==(str s1, str s2)
        {
            if (ReferenceEquals(s1, null) && ReferenceEquals(s2, null))
                return true;
            if (ReferenceEquals(s1, null) || ReferenceEquals(s2, null))
                return false;
            if (s1.length != s2.length)
                return false;
            for (int i = 0; i < s1.length; i++)
            {
                if (s1.characters[i] != s2.characters[i])
                    return false;
            }
            return true;
        }
        public static str operator +(str s1, str s2)
        {
            return s1.Concat(s2);
        }

        public static bool operator !=(str s1, str s2)
        {
            return !(s1 == s2);
        }

        public override bool Equals(object obj)
        {
            if (obj is str other)
            {
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            int hash = 17;
            foreach (char c in characters)
            {
                hash = hash * 31 + c.GetHashCode();
            }
            return hash;
        }

        public str Substring(int startIndex, int length)
        {
            if (startIndex < 0 || startIndex >= this.length || length < 0 || (startIndex + length) > this.length)
            {
                throw new ArgumentOutOfRangeException();
            }
            char[] result = new char[length];
            Array.Copy(characters, startIndex, result, 0, length);
            return new str(result);
        }

        public int IndexOf(char c)
        {
            for (int i = 0; i < this.length; i++)
            {
                if (characters[i] == c)
                    return i;
            }
            return -1;
        }

        public str ToUpper()
        {
            char[] result = new char[this.length];
            for (int i = 0; i < this.length; i++)
            {
                result[i] = char.ToUpper(characters[i]);
            }
            return new str(result);
        }

        public str ToLower()
        {
            char[] result = new char[this.length];
            for (int i = 0; i < this.length; i++)
            {
                result[i] = char.ToLower(characters[i]);
            }
            return new str(result);
        }

        public str Replace(char oldChar, char newChar)
        {
            char[] result = new char[this.length];
            for (int i = 0; i < this.length; i++)
            {
                result[i] = characters[i] == oldChar ? newChar : characters[i];
            }
            return new str(result);
        }

        public str Trim()
        {
            int start = 0;
            int end = this.length - 1;

            while (start <= end && char.IsWhiteSpace(characters[start])) start++;
            while (end >= start && char.IsWhiteSpace(characters[end])) end--;

            int length = end - start + 1;
            if (length < 0) length = 0;

            char[] result = new char[length];
            Array.Copy(characters, start, result, 0, length);
            return new str(result);
        }

        public bool Contains(char c)
        {
            return IndexOf(c) >= 0;
        }

        public bool StartsWith(str value)
        {
            if (value.length > this.length)
                return false;

            for (int i = 0; i < value.length; i++)
            {
                if (characters[i] != value.characters[i])
                    return false;
            }
            return true;
        }

        public bool EndsWith(str value)
        {
            if (value.length > this.length)
                return false;

            int startIndex = this.length - value.length;
            for (int i = 0; i < value.length; i++)
            {
                if (characters[startIndex + i] != value.characters[i])
                    return false;
            }
            return true;
        }

        public int IndexOf(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty.", nameof(value));

            char[] valueChars = value.ToCharArray();
            for (int i = 0; i <= this.length - valueChars.Length; i++)
            {
                bool match = true;
                for (int j = 0; j < valueChars.Length; j++)
                {
                    if (characters[i + j] != valueChars[j])
                    {
                        match = false;
                        break;
                    }
                }
                if (match)
                    return i;
            }
            return -1;
        }

        public int LastIndexOf(char c)
        {
            for (int i = this.length - 1; i >= 0; i--)
            {
                if (characters[i] == c)
                    return i;
            }
            return -1;
        }

        public str Insert(int startIndex, str value)
        {
            if (startIndex < 0 || startIndex > this.length)
            {
                throw new ArgumentOutOfRangeException();
            }
            char[] result = new char[this.length + value.length];
            Array.Copy(characters, 0, result, 0, startIndex);
            Array.Copy(value.characters, 0, result, startIndex, value.length);
            Array.Copy(characters, startIndex, result, startIndex + value.length, this.length - startIndex);
            return new str(result);
        }

        public str Remove(int startIndex)
        {
            if (startIndex < 0 || startIndex >= this.length)
            {
                throw new ArgumentOutOfRangeException();
            }
            char[] result = new char[startIndex];
            Array.Copy(characters, 0, result, 0, startIndex);
            return new str(result);
        }

        public str Remove(int startIndex, int count)
        {
            if (startIndex < 0 || startIndex >= this.length || count < 0 || (startIndex + count) > this.length)
            {
                throw new ArgumentOutOfRangeException();
            }
            char[] result = new char[this.length - count];
            Array.Copy(characters, 0, result, 0, startIndex);
            Array.Copy(characters, startIndex + count, result, startIndex, this.length - startIndex - count);
            return new str(result);
        }

        public str[] Split(params char[] separator)
        {
            List<str> result = new List<str>();
            int startIndex = 0;
            for (int i = 0; i < this.length; i++)
            {
                if (Array.Exists(separator, c => c == characters[i]))
                {
                    if (i > startIndex)
                    {
                        result.Add(this.Substring(startIndex, i - startIndex));
                    }
                    startIndex = i + 1;
                }
            }
            if (startIndex < this.length)
            {
                result.Add(this.Substring(startIndex, this.length - startIndex));
            }
            return result.ToArray();
        }

        public str PadLeft(int totalWidth, char paddingChar = ' ')
        {
            if (totalWidth < this.length)
                return this;

            char[] result = new char[totalWidth];
            int padding = totalWidth - this.length;
            for (int i = 0; i < padding; i++)
            {
                result[i] = paddingChar;
            }
            Array.Copy(characters, 0, result, padding, this.length);
            return new str(result);
        }

        public str TrimStart()
        {
            int start = 0;
            while (start < this.length && char.IsWhiteSpace(characters[start]))
            {
                start++;
            }

            return start >= this.length ? new str("") : this.Substring(start, this.length - start);
        }

        public str TrimEnd()
        {
            int end = this.length - 1;
            while (end >= 0 && char.IsWhiteSpace(characters[end]))
            {
                end--;
            }

            return end < 0 ? new str("") : this.Substring(0, end + 1);
        }

        public bool IsNullOrEmpty()
        {
            return this.length == 0;
        }

        public bool IsNullOrWhiteSpace()
        {
            return this.Trim().IsNullOrEmpty();
        }

        public str RemoveWhitespace()
        {
            char[] result = characters.Where(c => !char.IsWhiteSpace(c)).ToArray();
            return new str(result);
        }
    }


    public static class system
    {
        public static void print<T>(T message)
        {
            Console.Write(message);
        }
        public static void printr<T>(T message)
        {
            Console.WriteLine(message);
        }
        public static string scan()
        {
            return Console.ReadLine();
        }
    }
    public static class stringsys
    {
        public static string reverse(this string text)
        {
            try
            {
                string handle = null;
                for(int i = text.length() - 1; i >= 0; i--)
                {
                    handle += text[i];
                }
                return handle;
            }catch
            {
                return null;
            }
        }
        public static string trim(this string text)
        {
            try
            {
                return text.trimend().trimstart();
            }catch
            {
                return null;
            }
        }
        public static string trimend(this string text)
        {
            try
            {
               return text.reverse().trimstart().reverse();
            }
            catch
            {
                return null;
            }
        }
        public static string trimstart(this string text)
        {
            try
            {
                string handle = "";
                bool start = false;
                for (int i = 0; i < text.length(); i++)
                {
                    if (text[i] !=  ' ')
                    {
                        start = true;
                    }

                    if (start) handle += text[i];
                }
                return handle;
            }
            catch
            {
                return null;
            }
        }
        public static bool startswith(this string text,string value)
        {
            try
            {
                if (text.substring(0, value.length()) == value) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool endswith(this string text, string value)
        {
            try
            {
                if (text.substring(text.length() - value.length()) == value) return true;
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static int lastindex(this string text, char value)
        {
            try
            {
                return text.allindex(value)[text.allindex(value).length() - 1];
            }
            catch
            {
                return -1;
            }
        }
        public static int[] allindex(this string text,char value)
        {
            try
            {
                int[] hand = new int[text.count(value)];
                int val = 0;
                for(int i = 0;i < text.length();i++)
                {
                    if (text[i] == value)
                    {
                        hand[val] = i;
                        val++;
                    }
                }
                return hand;
            }
            catch
            {
                return new int[0];
            }
        }
        public static bool nullorempty(string text)
        {
            return text == null || text == "";
        }
        public static bool nullorwhitespace(string text)
        {
            try
            {
                string handle = null;
                for (int i = 0; i < text.length(); i++)
                {
                    handle += ' ';
                }
                if (handle == text || text == null) return true;
                else return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool contains(this string text, string value)
        {
            try
            {
                string handle = "";
                for(int i = 0; i < text.length(); i++)
                {
                    if (handle.length() != value.length())
                    {
                        handle += text[i];
                    }else
                    {
                        if (handle == value)
                        {
                            return true;
                        }
                        else handle = "";
                    }
                }
                return false;
            }catch
            {
                return false;
            }
        }
        public static string replace(this string text, string oldvalue, string newvalue)
        {
            try
            {
                string handler = null;
                int len = oldvalue.length();

                for (int i = 0; i < text.length(); i++)
                {
                    if (i <= text.length() - len && text.substring(i, len) == oldvalue)
                    {
                        handler += newvalue;
                        i += len - 1; 
                    }
                    else
                    {
                        handler += text[i];
                    }
                }
                return handler;
            }
            catch
            {
                return null;
            }
        }
        public static int count(this string text,char value)
        {
            try
            {
                int len = 0;
                for(int i = 0; i < text.length(); i++)
                {
                    if (text[i] == value) len++;
                }
                return len;
            }
            catch
            {
                return -1;
            }
        }
        public static string substring(this string text, int start,int length)
        {
            string hand = null;
            try
            {
                for (int i = start; i < start + length; i++)
                {
                    hand += text[i];
                }
                return hand;
            }
            catch
            {
                return null;
            }
        }
        public static string substring(this string text, int start)
        {
            string hand = null;
            try
            {
                for (int i = start; i < text.length(); i++)
                {
                    hand += text[i];
                }
                return hand;
            }
            catch
            {
                return null;
            }
        }
        unsafe public static int length(this string value)
        {      
            try
            {
                fixed (char* p = value)
                {
                    int length = 0;
                    char* temp = p;

                    while (*temp != '\0')
                    {
                        length++;
                        temp++;
                    }

                    return length;
                }
            }
            catch
            {
                return -1;
            }
        }
        public static bool equals(this object obj, object value)
        {
            return obj == value;
        }
        public static string padright(this string  text, int length,char value)
        {
            try
            {
                string handle = null;
                for (int i = 0;  i < length;  i++)
                {
                    handle += value;
                }
                return text + handle;
            }
            catch
            {
                return null;
            }
        }
        public static string padleft(this string text, int length, char value)
        {
            try
            {
                string handle = null;
                for (int i = 0; i < length; i++)
                {
                    handle += value;
                }
                return handle + text;
            }
            catch
            {
                return null;
            }
        }
    }
    public static class arraysys
    {
        public static string[] split(this string text, char value) 
        {
            string[] handle = new string[0];
            string values = null;
            int len = 0;
            try
            {
                for (int i = 0; i < text.length(); i++)
                {
                    if (text[i] != value)
                    {
                        values += text[i];
                    }
                    else
                    {
                        resize(ref handle, handle.length() + 1);
                        handle[len] = values;
                        len++;
                        values = null;
                    }
                }
                resize(ref handle, handle.length() + 1);
                handle[len] = values;
                return handle;
            }
            catch
            {
                return new string[0];
            }
        }
        public static string join<T, C>(this T[] array, C value)
        {
            string handle = null;
            try
            {
                for (int i = 0; i < array.length(); i++)
                {
                    handle += array[i];
                    if (i + 1 != array.length())
                    {
                        handle += value;
                    }
                }
                return handle;
            }
            catch
            {
                return null;
            }
        }
        public static void reverse<T>(ref T[] array)
        {
            try
            {
                T[] values = new T[array.length()];
                int rev = 0;
                for (int i = array.length() - 1; i >= 0; i--)
                {
                    values[rev++] = array[i];
                }
                array = values;
            }
            catch
            {
                return;
            }

        }
        public static int length<T>(this T[] array)
        {
            int len = 0;
            for (int i = 0; ; i++)
            {
                try
                {
                    T c = array[i];
                    len++;
                }
                catch
                {
                    break;
                }
            }
            return len;
        }
        public static void resize<T>(ref T[] array, int newsize)
        {
            try
            {
                T[] arr = new T[newsize];
                if (newsize > array.length())
                {
                    for (int i = 0; i < array.length(); i++)
                    {
                        arr[i] = array[i];
                    }
                }
                else
                {
                    for (int i = 0; i < arr.length(); i++)
                    {
                        arr[i] = array[i];
                    }
                }
                array = arr;
            }
            catch
            {
                return;
            }
        }
        public static int[] allindex<T>(this T[] array, T value)
        {
            try
            {
                int[] values = new int[0];
                int rev = 0;
                for (int i = 0; i < array.length(); i++)
                {
                    if (array[i].equals(value))
                    {
                        resize(ref values, values.length() + 1);
                        values[rev] = i;
                        rev++;

                    }
                }
                for (int i = 0; i < values.length(); i++) values[i] = values[i];
                return values;
            }
            catch
            {
                return new int[0];
            }
        }
        public static int firstindex<T>(this T[] array, T value)
        {
            try
            {
                return array.allindex(value)[0];
            }
            catch
            {
                return -1;
            }
        }
        public static int lastindex<T>(this T[] array, T value)
        {
            try
            {
                return array.allindex(value)[array.allindex(value).length() - 1];
            }
            catch
            {
                return -1;
            }
        }
        public static bool contains<T>(this T[] array, T value)
        {
            try
            {
                if (array == null) return false;
                for (int i = 0; i < array.length(); i++)
                {
                    if (array[i].equals(value))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
