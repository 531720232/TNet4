﻿/****************************************************************************
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
using System;
using TNet.Model;
using TNet.Event;

namespace TNet.Task
{
	/// <summary>
	/// Task status.
	/// </summary>
    public enum TaskStatus
    {
        /// <summary>
        /// 不可接
        /// </summary>
        NoTake = 0,
        /// <summary>
        /// 可接
        /// </summary>
        AllowTake,
        /// <summary>
        /// 已接
        /// </summary>
        Taked,
        /// <summary>
        /// 完成
        /// </summary>
        Completed,
        /// <summary>
        /// 结束
        /// </summary>
        Close,
        /// <summary>
        /// 禁用
        /// </summary>
        Disable
    }

    /// <summary>
    /// 游戏任务信息
    /// </summary>
    public interface ITaskItem
    {
		/// <summary>
		/// Gets or sets the user I.
		/// </summary>
		/// <value>The user I.</value>
        int UserID { get; set; }
		/// <summary>
		/// Gets or sets the task I.
		/// </summary>
		/// <value>The task I.</value>
        int TaskID { get; set; }
		/// <summary>
		/// Gets or sets the status.
		/// </summary>
		/// <value>The status.</value>
        TaskStatus Status { get; set; }
		/// <summary>
		/// Gets or sets the create date.
		/// </summary>
		/// <value>The create date.</value>
        DateTime CreateDate { get; set; }
    }
}