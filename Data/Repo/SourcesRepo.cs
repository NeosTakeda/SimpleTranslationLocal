using SimpleTranslationLocal.Data.DataModel;
using SimpleTranslationLocal.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleTranslationLocal.Data.Repo {
    class SourcesRepo : IBasicRepo<SourceData> {

        #region Constructor
        public SourcesRepo(DictionaryDatabase database) : base(database) { }
        #endregion

        public override bool Create() {
            throw new NotImplementedException();
        }

        public override void CreateTable() {
            throw new NotImplementedException();
        }

        public override void DeleteBySourceId(long id) {
            throw new NotImplementedException();
        }

        public override long Insert() {
            throw new NotImplementedException();
        }

        public override void SetDataModel(SourceData model) {
            throw new NotImplementedException();
        }
    }
}
