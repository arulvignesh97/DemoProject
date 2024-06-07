using CTS.Applens.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CTS.Applens.WorkProfiler.DAL.BaseDetails
{
    public class AseKeyDetail
    {
        public static readonly byte[] AesKeyConstVal = new CacheManager().GetOrCreate<byte[]>("aesKeyconst", ()=> new byte[32], CacheDuration.Long);
        public static string AesKeyconstAPIval = new CacheManager().GetOrCreate<string>("aesKeyconstAPI", ()=>string.Empty, CacheDuration.Long);
    }
}
