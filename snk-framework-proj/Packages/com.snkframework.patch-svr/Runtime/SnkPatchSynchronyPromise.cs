using System.Collections.Generic;
using System.Linq;
using SnkFramework.PatchBuilder.Runtime.Core;

namespace SnkFramework.PatchService.Runtime
{
    public class SnkPatchSynchronyPromise
    {
        public List<SnkSourceInfo> SourceInfoList;

        public SnkPatchSynchronyPromise()
        {
            this.SourceInfoList = new List<SnkSourceInfo>();
        }
 
        public long GetTotalSize() =>this.SourceInfoList.Sum(a => a.size);
        
    }
}