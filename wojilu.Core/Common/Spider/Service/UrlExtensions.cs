using System;

namespace BlogGather
{
    //public static class UrlExtensions
    //{
    //    #region Readonly & Static Fields

    //    private static readonly string s_HttpScheme = Uri.UriSchemeHttp + "://";
    //    private static readonly string s_HttpsScheme = Uri.UriSchemeHttp + "://";

    //    #endregion

    //    #region Class Methods

    //    public static string NormalizeUrl(this string url, string baseUrl)
    //    {
    //        try
    //        {
    //            if (string.IsNullOrEmpty(url))
    //            {
    //                return baseUrl;
    //            }

    //            if (url.IndexOf("..") != -1 ||
    //                url.StartsWith("/") ||
    //                !url.StartsWith(s_HttpScheme, StringComparison.OrdinalIgnoreCase) ||
    //                !url.StartsWith(s_HttpsScheme, StringComparison.OrdinalIgnoreCase))
    //            {
    //                url = new Uri(new Uri(baseUrl), url).AbsoluteUri;
    //            }

    //            if (Uri.IsWellFormedUriString(url, UriKind.Relative))
    //            {
    //                if (string.IsNullOrEmpty(baseUrl))
    //                {
    //                    Uri absoluteBaseUrl = new Uri(baseUrl, UriKind.Absolute);
    //                    return new Uri(absoluteBaseUrl, url).ToString();
    //                }

    //                return new Uri(url, UriKind.Relative).ToString();
    //            }

    //            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
    //            {
    //                // Only handle same schema as base uri
    //                Uri baseUri = new Uri(baseUrl);
    //                Uri uri = new Uri(url);
    //                bool schemaMatch;

    //                // Special case for http/https
    //                //if (baseUri.Scheme.IsIn(Uri.UriSchemeHttp, Uri.UriSchemeHttps))
    //                if (Uri.UriSchemeHttps.Contains(Uri.UriSchemeHttp))
    //                {
    //                    schemaMatch = string.Compare(Uri.UriSchemeHttp, uri.Scheme, StringComparison.OrdinalIgnoreCase) == 0 ||
    //                        string.Compare(Uri.UriSchemeHttps, uri.Scheme, StringComparison.OrdinalIgnoreCase) == 0;
    //                }
    //                else
    //                {
    //                    schemaMatch = string.Compare(baseUri.Scheme, uri.Scheme, StringComparison.OrdinalIgnoreCase) == 0;
    //                }

    //                if (schemaMatch)
    //                {
    //                    return new Uri(url, UriKind.Absolute).ToString();
    //                }
    //            }

    //            return null;
    //        }
    //        catch
    //        {
    //            return null;
    //        }
            
    //    }

    //    #endregion
    //}
}