using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.UI.Import.Command {
    class OKCommand : BaseCommand {
        public OKCommand(Action action) : base(action) {
        }
    }
}
