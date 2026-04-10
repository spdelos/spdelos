using System.Text.RegularExpressions;

namespace CSDBPortal.Services
{
    /// <summary>
    /// Applies a DRAFT watermark to XML content in-memory before encryption.
    /// Mirrors CSDBLite's WatermarkService but operates on strings rather than files.
    /// </summary>
    public static class NavWatermarkService
    {
        private const string CssWatermark = @"
<style>
body::before {
    content: 'DRAFT';
    position: fixed;
    top: 50%;
    left: 50%;
    transform: translate(-50%, -50%) rotate(-45deg);
    font-size: 15vw;
    font-weight: bold;
    color: rgba(255, 0, 0, 0.15);
    pointer-events: none;
    z-index: 9999;
    white-space: nowrap;
}
</style>";

        private const string XmlDraftComment =
            "<!-- DRAFT VERSION - This is a draft document -->";

        /// <summary>
        /// Returns the XML content with a DRAFT watermark applied.
        /// HTML-embedded XML gets a CSS overlay; plain S1000D XML gets an XML comment.
        /// </summary>
        public static string WatermarkXml(string xmlContent)
        {
            if (string.IsNullOrWhiteSpace(xmlContent))
                return xmlContent;

            // HTML-embedded XML: inject CSS watermark before </head>; fall back to after <body>
            if (xmlContent.Contains("<html", StringComparison.OrdinalIgnoreCase))
            {
                var headClose = Regex.Match(xmlContent, @"</head>", RegexOptions.IgnoreCase);
                if (headClose.Success)
                {
                    return xmlContent.Insert(headClose.Index, CssWatermark + "\n");
                }

                var bodyOpen = Regex.Match(xmlContent, @"<body[^>]*>", RegexOptions.IgnoreCase);
                if (bodyOpen.Success)
                {
                    return xmlContent.Insert(bodyOpen.Index + bodyOpen.Length, "\n" + CssWatermark);
                }

                // No head or body tag found — append to end
                return xmlContent + CssWatermark;
            }

            // Plain S1000D XML: insert comment after the XML declaration (<?xml ... ?>)
            var xmlDecl = Regex.Match(xmlContent, @"<\?xml[^?]*\?>");
            if (xmlDecl.Success)
            {
                int insertAt = xmlDecl.Index + xmlDecl.Length;
                return xmlContent.Insert(insertAt, "\n" + XmlDraftComment);
            }

            // No XML declaration — prepend the comment
            return XmlDraftComment + "\n" + xmlContent;
        }
    }
}
