using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SimpleTranslationLocal.UI.Import.Command {
    class ImportDictionaryCommand : BaseCommand {

        public ImportDictionaryCommand(Action action) : base(action) {
        }
    }
}
