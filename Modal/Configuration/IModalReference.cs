using MC.Modal.Services;

namespace MC.Modal;

public interface IModalReference
{
    Task<ModalResult> Result { get; }

    void Close();
    void Close(ModalResult result);
}