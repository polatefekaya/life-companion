using System;
using System.Collections.Generic;

namespace LifeCompanion.Domain.Options {
    public class GeminiOptions {
        public const string TextOnly = "TextOnly";
        public const string TextAndImage = "TextAndImage";

        public string ContentType { get; set; } = String.Empty;
        public string Model { get; set; } = String.Empty;
        public string ApiVersion { get; set; } = String.Empty;
        public string RequestType { get; set; } = String.Empty;
    }
}
