// Set toastr options once during initialization
toastr.options = {
    closeButton: true,
    debug: false,
    newestOnTop: true,
    progressBar: true,
    positionClass: "toast-top-right",
    preventDuplicates: false,
    onclick: null,
    showDuration: 300,
    hideDuration: 1000,
    timeOut: 4000,
    extendedTimeOut: 1000,
    showEasing: "swing",
    hideEasing: "linear",
    showMethod: "fadeIn",
    hideMethod: "fadeOut"
};

// Notification function
function fnGetNotified(type, message) {
    const validTypes = ["success", "error", "info", "warning"];
    if (validTypes.includes(type) && message?.trim()) {
        toastr[type](message.trim());
    }
}