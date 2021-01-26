using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.AppCommon {
    static class Env {
        public enum EnvType {
            Release, Debug, Stub
        }
#if DEBUG
        public static EnvType Current = EnvType.Debug;
#elif STUB
        public static EnvType Current = EnvType.Stub;
#else
        public static EnvType Current = EnvType.Release;
#endif
    }
}
