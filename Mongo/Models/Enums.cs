using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mongo.Models
{
    public enum LoginStatus
    {
        Success = 0,
        LockedOut = 1,
        RequiresVerification = 2,
        Failure = 3
    }

    public enum TypeImageSend
    {
        ChatMessage = 1,
        PostTimeLine = 2,
        Galery = 3,
    }

    public enum TipoSugar
    {
        SugarDaddy = 1,
        SugarBaby = 2,
    }
}
