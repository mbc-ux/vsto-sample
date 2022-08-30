using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vsto.Sample.Client
{
    public interface ICanPublishData
    {
        event Action<object> DataPublished;
    }
}
