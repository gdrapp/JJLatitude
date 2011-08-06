using System;
using System.Text;

namespace Matlus.FederatedIdentity
{
  public class AuthorizeHeader
  {
    public string Realm { get; private set; }
    public string ConsumerKey { get; private set; }
    public string SignatureMethod { get; private set; }
    public string Signature { get; private set; }
    public string Timestamp { get; private set; }
    public string Nounce { get; private set; }
    public string Version { get; private set; }
    public string Callback { get; private set; }
    public string Token { get; private set; }
    public string Verifier { get; private set; }

    public AuthorizeHeader(string realm, string consumerKey, string signatureMethod, string signature, string timestamp, string nounce, string version)
    {
      Realm = realm;
      ConsumerKey = consumerKey;
      SignatureMethod = signatureMethod;
      Signature = signature;
      Timestamp = timestamp;
      Nounce = nounce;
      Version = version;
    }

    public AuthorizeHeader(string realm, string consumerKey, string signatureMethod, string signature, string timestamp, string nounce, string version, string callback)
      : this(realm, consumerKey, signatureMethod, signature, timestamp, nounce, version)
    {
      Callback = callback;
    }

    public AuthorizeHeader(string realm, string consumerKey, string signatureMethod, string signature, string timestamp, string nounce, string version, string token, string verifier)
      : this(realm, consumerKey, signatureMethod, signature, timestamp, nounce, version)
    {
      Token = token;
      Verifier = verifier;
    }

    public override string ToString()
    {
      var sb = new StringBuilder();
      sb.Append("OAuth ");
      sb.AppendFormat("realm=\"{0}\", ", Realm);
      sb.AppendFormat("{0}=\"{1}\", ", OAuthProtocolParameter.ConsumerKey.GetStringValue(), OAuthUtils.UrlEncode(ConsumerKey));
      sb.AppendFormat("{0}=\"{1}\", ", OAuthProtocolParameter.SignatureMethod.GetStringValue(), SignatureMethod);
      sb.AppendFormat("{0}=\"{1}\", ", OAuthProtocolParameter.Signature.GetStringValue(), OAuthUtils.UrlEncode(Signature));
      sb.AppendFormat("{0}=\"{1}\", ", OAuthProtocolParameter.Timestamp.GetStringValue(), Timestamp);
      sb.AppendFormat("{0}=\"{1}\", ", OAuthProtocolParameter.Nounce.GetStringValue(), OAuthUtils.UrlEncode(Nounce));
      sb.AppendFormat("{0}=\"{1}\", ", OAuthProtocolParameter.Version.GetStringValue(), Version);
      if (!String.IsNullOrEmpty(Callback))
        sb.AppendFormat("{0}=\"{1}\", ", OAuthProtocolParameter.Callback.GetStringValue(), OAuthUtils.UrlEncode(Callback));
      if (!String.IsNullOrEmpty(Token))
        sb.AppendFormat("{0}=\"{1}\", ", OAuthProtocolParameter.Token.GetStringValue(), OAuthUtils.UrlEncode(Token));
      if (!String.IsNullOrEmpty(Verifier))
        sb.AppendFormat("{0}=\"{1}\", ", OAuthProtocolParameter.Verifier.GetStringValue(), OAuthUtils.UrlEncode(Verifier));

      sb = sb.Remove(sb.Length - 2, 2);
      return sb.ToString();
    }
  }
}