using Csc.IntelliSchool.Data;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Csc.IntelliSchool.WebService.Services.Model {
  [MessageContract]
  public class RemoteFile : IDisposable {
    [MessageHeader(MustUnderstand = true)]
    public RemoteFileType Type { get; set; }
    [MessageHeader(MustUnderstand = true)]
    public string Filename { get; set; }
    [MessageHeader(MustUnderstand = true)]
    public long Length { get; set; }
    [MessageBodyMember(Order = 1)]
    public Stream Stream { get; set; }

    public void Dispose() {
      if (Stream != null) {
        Stream.Dispose();
        Stream = null;
      }
    }
  }
}