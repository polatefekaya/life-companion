using System;
using System.Collections.Generic;

namespace LifeCompanion.Domain.Data.Models {
    public class TextOnlyGeminiEntity {

        public Content[] contents { get; set; }

        public class Content {
            public Part[] parts { get; set; }
        }

        public class Part {
            public string text { get; set; }
        }

    }
}
