﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TNet.Model;

namespace TNet.Com
{

    ///<summary>
    ///Lua操作工具类
    ///</summary>
    [Serializable, ProtoContract]
    public static class LuaUtils
    {
        /// <summary>
        /// 格式化内容为Label
        /// </summary>
        /// <example>
        /// color:#ffffff 或者 #fff
        /// </example>
        /// <param name="value"></param>
        /// <param name="color">带#号的16进制颜色</param>
        /// <returns></returns>
        public static string FormatLabel(string value, string color)
        {
            return FormatLabel(value, ParseRgb(color));
        }

        /// <summary>
        /// 格式化内容为Label
        /// </summary>
        /// <param name="value"></param>
        /// <param name="rgb">范围在0-255之间</param>
        /// <returns></returns>
        public static string FormatLabel(string value, RgbColor rgb)
        {
            string color = string.Format("{0},{1},{2}", rgb.R, rgb.G, rgb.B);
            return string.Format("<label color='{0}'>{1}</label>", color, value);
        }

        ///<summary>
        /// 解释成RGB颜色
        ///</summary>
        ///<param name="colorHtml">带#号的16进制颜色</param>
        ///<returns></returns>
        private static RgbColor ParseRgb(string colorHtml)
        {
            int red, green, blue = 0;
            char[] rgb;
            string color = colorHtml.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return new RgbColor(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return new RgbColor(red, green, blue);
                default:
                    throw new ArgumentOutOfRangeException("colorHtml", colorHtml, "The color is not a valid format");
            }
        }
    }
}
