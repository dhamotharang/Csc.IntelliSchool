using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Csc.Components.Services {
  public class RequestHeadersClientInspector : IClientMessageInspector {
    public HeadersCallback HeadersCallback { get; set; }

    public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, IClientChannel channel) {
      IDictionary<string, string> dict = null;

      if (HeadersCallback != null)
        dict = HeadersCallback();

      if (dict == null || dict.Count == 0)
        return null;

      if (request.Properties.Count == 0 || request.Properties[HttpRequestMessageProperty.Name] == null) {
        foreach (var val in dict) {
          var property = new HttpRequestMessageProperty();
          property.Headers[val.Key] = val.Value;
          request.Properties.Add(HttpRequestMessageProperty.Name, property);
        }
      } else {
        foreach (var val in dict) {
          ((HttpRequestMessageProperty)request.Properties[HttpRequestMessageProperty.Name]).Headers[val.Key] = val.Value;
        }
      }

      return null;
    }

    public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState) {
    }
  }
}
