﻿using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;
using TNet.Cache.Generic;
using TNet.Extend;
using TNet.Model;

namespace TNet.Com.Model
{
    /// <summary>
    /// 题库数据
    /// </summary>
    [Serializable, ProtoContract]
    [EntityTable(AccessLevel.ReadOnly, "", "")]
    public class QuestionData : ShareEntity
    {
        ///<summary>
        ///</summary>
        public QuestionData()
            : base(AccessLevel.ReadOnly)
        {
        }

        #region auto-generated Property
        private Int32 _ID;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(1)]
        [EntityFieldExtend]
        [EntityField(true)]
        public Int32 ID
        {
            get
            {
                return _ID;
            }
            private set
            {
                SetChange("ID", value);
            }
        }
        private String _Topic;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(2)]
        [EntityFieldExtend]
        [EntityField]
        public String Topic
        {
            get
            {
                return _Topic;
            }
            private set
            {
                SetChange("Topic", value);
            }
        }
        private CacheList<QuestionOption> _Options;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(3)]
        [EntityFieldExtend]
        [EntityField(true, ColumnDbType.Text)]
        public CacheList<QuestionOption> Options
        {
            get
            {
                return _Options;
            }
            private set
            {
                SetChange("Options", value);
            }
        }
        private string _Answer;
        /// <summary>
        /// 
        /// </summary>        
        [ProtoMember(4)]
        [EntityFieldExtend]
        [EntityField]
        public string Answer
        {
            get
            {
                return _Answer;
            }
            private set
            {
                SetChange("Answer", value);
            }
        }
        /// <summary>
        /// 对象索引器属性
        /// </summary>
        /// <returns></returns>
        /// <param name="index">Index.</param>
        protected override object this[string index]
        {
            get
            {
                #region
                switch (index)
                {
                    case "ID": return ID;
                    case "Topic": return Topic;
                    case "Options": return Options;
                    case "Answer": return Answer;
                    default: throw new ArgumentException(string.Format("QuestionData index[{0}] isn't exist.", index));
                }
                #endregion
            }
            set
            {
                #region
                switch (index)
                {
                    case "ID":
                        _ID = value.ToInt();
                        break;
                    case "Topic":
                        _Topic = value.ToNotNullString();
                        break;
                    case "Options":
                        _Options = ConvertCustomField<CacheList<QuestionOption>>(value, index);
                        break;
                    case "Answer":
                        _Answer = value.ToNotNullString();
                        break;
                    default: throw new ArgumentException(string.Format("QuestionData index[{0}] isn't exist.", index));
                }
                #endregion
            }
        }

        #endregion
    }
}
