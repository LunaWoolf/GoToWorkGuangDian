﻿/**
 * Copyright (c) 2012 Yang Kuang
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 *
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
**/

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;
using Pinyin4net.Format;
using UnityEngine;
namespace Pinyin4net
{
    /// <summary>
    /// Summary description for PinyinHelper.
    /// </summary>
    public class PinyinHelper
    {
        private static Dictionary<string, string> dict;

        /// <summary>
        /// We don't need any instances of this object.
        /// </summary>
        private PinyinHelper()
        {
        }

        /// <summary>
        /// Load unicode-pinyin map to memery while this class first use.
        /// </summary>
        static PinyinHelper()
        {
            dict = new Dictionary<string, string>();

            //Load a text file (Assets/Resources/Text/textFile01.txt)
            string fileName = "unicode_to_hanyu_pinyin";//"unicode_to_hanyu_pinyin.xml";
            TextAsset textAsset = Resources.Load<TextAsset>(fileName);

            //var name = "Pinyin4net.Resources.unicode_to_hanyu_pinyin.xml";
            //var assembly = Assembly.GetExecutingAssembly();

            //using Stream stream = assembly.GetManifestResourceStream(name);
            if (textAsset == null)
            {
                Debug.Log("Nofile");
              throw new InvalidOperationException();
            }

            //if (textAsset != null)
            //{
              

            //}
            // Convert TextAsset to MemoryStream
            MemoryStream memoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(textAsset.text));

            // Load the XML document using XDocument.Load
            var doc = XDocument.Load(memoryStream);


            //var doc = XDocument.Load(stream);
            var query =
                from item in doc.Root?.Descendants("item")
                select new
                {
                    Unicode = (string)item.Attribute("unicode"),
                    Hanyu = (string)item.Attribute("hanyu")
                };
            foreach (var item in query)
                if (item.Hanyu.Length > 0)
                    dict.Add(item.Unicode, item.Hanyu);
        }

        /// <summary>
        /// Get all Hanyu pinyin of a single Chinese character (both
        /// Simplified Chinese and Traditional Chinese).
        /// 
        /// This function is same with: 
        ///     ToHanyuPinyinStringArray(ch, new HanyuPinyinOutputFormat());
        ///
        /// For example, if the input is '偻', the return will be an array with 
        /// two Hanyu pinyin strings: "lou2", "lv3". If the input is '李', the
        /// return will be an array with one Hanyu pinyin string: "li3".
        /// </summary>
        /// <param name="ch">The given Chinese character</param>
        /// <returns>A string array contains all Hanyu pinyin presentations; return 
        /// null for non-Chinese character.</returns>
        public static string[] ToHanyuPinyinStringArray(char ch)
        {
            return ToHanyuPinyinStringArray(ch, new HanyuPinyinOutputFormat());
        }

        /// <summary>
        /// Get all Hanyu pinyin of a single Chinese character (both
        /// Simplified Chinese and Traditional Chinese).
        /// </summary>
        /// <param name="ch">The given Chinese character</param>
        /// <param name="format">The given output format</param>
        /// <returns>A string array contains all Hanyu pinyin presentations; return 
        /// null for non-Chinese character.</returns>
        public static string[] ToHanyuPinyinStringArray(
            char ch, HanyuPinyinOutputFormat format)
        {
            return GetFomattedHanyuPinyinStringArray(ch, format);
        }

        #region Private Functions
        private static string[] GetFomattedHanyuPinyinStringArray(
            char ch, HanyuPinyinOutputFormat format)
        {
            string[] unformattedArr = GetUnformattedHanyuPinyinStringArray(ch);
            if (null != unformattedArr)
            {
                for (int i = 0; i < unformattedArr.Length; i++)
                {
                    unformattedArr[i] = PinyinFormatter.FormatHanyuPinyin(unformattedArr[i], format);
                }
            }

            return unformattedArr;
        }

        private static string[] GetUnformattedHanyuPinyinStringArray(char ch)
        {
            string code = String.Format("{0:X}", (int)ch).ToUpper();
#if DEBUG
            Console.WriteLine(code);
#endif
            if (dict.ContainsKey(code))
            {
                return dict[code].Split(',');
            }

            return null;
        }
        #endregion
    }
}