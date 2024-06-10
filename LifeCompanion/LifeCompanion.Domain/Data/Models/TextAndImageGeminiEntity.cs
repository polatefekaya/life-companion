using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;

namespace LifeCompanion.Domain.Data.Models {
    public class TextAndImageGeminiEntity {
        public Content[] contents { get; set; }

        public class Content {
            public Part[] parts { get; set; }
        }

        public class Part {
            public string text { get; set; }
            public Inline_Data inline_data { get; set; }
        }

        public class Inline_Data {
            public string mime_type { get; set; }
            public string data { get; set; }
        }

    }
}
