function ModalConfirmationShow() {
    var confirmationModal = new bootstrap.Modal(document.getElementById('ModalGeneralConfirm'));
    confirmationModal.show();
}
function ModalConfirmationHide() {
    //var confirmationModal = new bootstrap.Modal(document.getElementById('ModalGeneralConfirm'));
    //confirmationModal.hide();
    bootstrap.Modal.getOrCreateInstance(document.getElementById('ModalGeneralConfirm')).hide();
}