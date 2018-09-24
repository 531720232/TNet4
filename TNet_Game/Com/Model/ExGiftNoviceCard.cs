using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TNet.Com.Model
{
    [ProtoBuf.ProtoContract]
    public class ExGiftNoviceCard : GiftNoviceCard
    {
       [ProtoBuf.ProtoMember(1)]
        public override string CardNo { get ; set; }
        public override string GiftType { get; set; }
        public override int UserId { get; set; }
        public override DateTime ActivateDate { get; set; }//{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool IsInvalid { get; set; }// { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override string CreateIp { get; set; }//{ get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override DateTime CreateDate { get; set; }// { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override int GetIdentityId()
        {
          
            return 1;
        }
       public ExGiftNoviceCard()
        {
            base.SetPropertyValue("xsk", this);
        }
        public override string PersonalId => base.PersonalId;
    }
}
