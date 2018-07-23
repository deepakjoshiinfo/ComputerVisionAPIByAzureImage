using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShowImages.Utility
{
    public class AppConstant
    {
        public static string CloudBlobContainer = "items";
        public static string CloudBlobAccountName = "deeppawncloudcustomer";
        public static string CloudBlobContainerURL = string.Format("https://{0}.blob.core.windows.net/{1}", CloudBlobAccountName, CloudBlobContainer);
        public static string CloudBlobKey = "r0OatxcT/TZ4AMOFgjZR81oQb4Jt1vBUNupANKXnwP9PA7SNGnvylxF36BsDdIOm0lUx+OKZahEfO17UIJnDFQ==";
    }
}
