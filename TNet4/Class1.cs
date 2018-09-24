using System;
using TNet.Script;
using TNet.Service;
using TNet.Extend;
using TNet.Context_;

namespace Game.Script
{
    public class AccountInfo : TNet.Model.ShareEntity
    {
        public enum Card_Type
        {
            Mainland=0,//内陆
            HMT=1,//港澳台

        }
        public enum Sex_Type
        {
            Male = 0,
            Female = 1,
            UnKnow=2

        }
        public string NickName { get; set; }
        public string RawPassword { get; set; }
        public string Password { get; set; }
        public DateTime RegTime { get; set; }
        public DateTime LoginTime { get; set; }
        public Sex_Type Sex { get; set; }
       public Card_Type CardType { get; set; }
        public string IdCard { get; set; }

    }
    [AutoRun]
    public class Class1
    {

        public static TNet.Cache.Generic.ShareCacheStruct<AccountInfo> acc = new TNet.Cache.Generic.ShareCacheStruct<AccountInfo>();
        

public static void Main(string[] args)

        {
   
            Console.WriteLine("f25");
        }
    }

    
    public class Action2017 : TNet.Service.BaseStruct
    {
        public Action2017(TNet.Service.ActionGetter get) :base(11,get)
        {
          
        }
        public string Nick;
        public string Pass;
        public int Sex;
        public int CardType;
        public string IdCard;
        public string error;
        public override bool GetUrlElement()
        {
           if(httpGet.GetString("nick",ref Nick)&&httpGet.GetString("pass", ref Pass)
                && httpGet.GetInt("sex", ref Sex) && httpGet.GetInt("cardtype", ref Sex) &&
            httpGet.GetString("idcard", ref IdCard))
                {

                return true;
            }
         
            return false;
        }
        public override void BuildPacket()
        {
            var v = Reg();
         
            PushIntoStack(error);
            if(v==null)
            {

                PushIntoStack(false);

            }
          //  base.BuildPacket();
        }
        public  AccountInfo Reg()
        {
         
        
            if (!Class1.acc.IsExist(x => x.NickName == Nick))
            {
                var new_acc = new AccountInfo();
                new_acc.RegTime = DateTime.Now;
                new_acc.RawPassword = Pass;
                new_acc.Sex = Sex.ToEnum<AccountInfo.Sex_Type>();
                new_acc.IdCard = IdCard;
                new_acc.CardType = CardType.ToEnum<AccountInfo.Card_Type>();
                Class1.acc.Add(new_acc);
                error = "null";
                return new_acc;
            }
            else
            {
                error = "账号已存在";
                return null;
            }
               
        }
        public override bool TakeAction()
        {
            Console.WriteLine("f25");
            return true;
        }
    }
    public class Action5 : TNet.Service.BaseStruct
    {

        public MobileType type;
        public string client;
        public string uid;
        public Action5(TNet.Service.ActionGetter get) : base(5, get)
        {

        }
        public override bool GetUrlElement()
        {
  
          type=  httpGet.GetInt("MobileType").ToEnum<MobileType>();
          client=  httpGet.GetString("ClientAppVersion");
            uid = httpGet.GetString("DeviceID");
   
        
                return base.GetUrlElement();
        }
        public override bool TakeAction()
        {
        
            Console.WriteLine("f25");
            return true;
        }

        
    }
}

