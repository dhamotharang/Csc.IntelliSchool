using System.Linq;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Csc.Components.Services {
  public class CustomEndpointBehavior : IEndpointBehavior {
    public HeadersCallback HeadersCallback { get; set; }
    public int? MaxItemsInObjectGraph { get; set; }


    public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) {
      ModifyDataContractSerializerBehavior(endpoint, MaxItemsInObjectGraph);
      if (HeadersCallback != null)
        clientRuntime.MessageInspectors.Add(new RequestHeadersClientInspector() { HeadersCallback = this.HeadersCallback });
    }

    public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) {
    }

    public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) {
      ModifyDataContractSerializerBehavior(endpoint, MaxItemsInObjectGraph);
    }


    private static void ModifyDataContractSerializerBehavior(ServiceEndpoint endpoint, int? maxItems) {
      if (maxItems == null)
        return;

      foreach (var behavior in endpoint.Contract.Operations.Select(operation => operation.Behaviors.Find<DataContractSerializerOperationBehavior>())) {
        behavior.MaxItemsInObjectGraph = maxItems.Value;
      }
    }

    public void Validate(ServiceEndpoint endpoint) {
    }
  }
}
