function fnValidateEmail(email) {
    if (!email || typeof email !== 'string') return false;

    const regex = /^\w+([-+.'']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    return regex.test(email.trim());
}

function toggleLoading(show) {
    $('#loading-element').toggle(show);
}

function resetPagination() {
    $('#Pagination_Skip').val(0);
    $('#Pagination_TotalRecord').val(0);
}

function fnHandleAjaxRequest({ url, method = 'GET', dataType = 'html', data = null, contentType = 'application/json', onSuccess, onError }) {
    toggleLoading(true);
    $.ajax({
        url: url,
        method: method,
        contentType: contentType,
        dataType: dataType,
        data: data,
        success: function (response) {
            toggleLoading(false);
            if (typeof onSuccess === 'function') {
                onSuccess(response);
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            toggleLoading(false);

            let errorMsg = "An unexpected error occurred.";

            // Try to extract meaningful message
            if (jqXHR.responseJSON && jqXHR.responseJSON.msg) {
                errorMsg = jqXHR.responseJSON.msg;
            } else if (jqXHR.responseText) {
                errorMsg = jqXHR.responseText;
            } else if (errorThrown) {
                errorMsg = errorThrown;
            }

            if (typeof onError === 'function') {
                onError(errorMsg);
            }
        }
    });
}

function triggerPageNavigation(btn) {
    const config = window.paginationConfig;
    const currentSkip = parseInt($('#Pagination_Skip').val()) || 0;

    let newSkip = currentSkip;

    switch (btn) {
        case "First":
            if (config.currentPage > 1) newSkip = 0;
            break;

        case "Next":
            if (config.currentPage < config.totalPage) newSkip += config.pageSize;
            break;

        case "Previous":
            if (currentSkip >= config.pageSize) newSkip -= config.pageSize;
            break;

        case "Last":
            if (config.currentPage < config.totalPage) newSkip = (config.totalPage - 1) * config.pageSize;
            break;

        default:
            return;
    }

    if (newSkip !== currentSkip || btn === "First") {
        $('#Pagination_Skip').val(newSkip);
        $('#frmList').submit();
    }
}

function getMinutes(timeStr) {
    if (!timeStr || !timeStr.includes(":"))
        return 0;

    const [hours, minutes] = timeStr.split(":").map(v => parseInt(v, 10));
    return (isNaN(hours) ? 0 : hours) * 60 + (isNaN(minutes) ? 0 : minutes);
}

function formatToHHMM(totalMinutes) {
    const hours = Math.floor(totalMinutes / 60);
    const minutes = totalMinutes % 60;
    return `${hours.toString().padStart(2, '0')}:${minutes.toString().padStart(2, '0')}`;
}

function parseHHMMToMinutes(timeStr) {
    if (!timeStr || !/^\d{2}:\d{2}$/.test(timeStr)) return NaN;
    const [hh, mm] = timeStr.split(':').map(Number);
    return hh * 60 + mm;
}

function parseTimeToMinutes(timeStr) {
    const parts = timeStr.split(':');
    if (parts.length !== 2) return NaN;
    const hours = parseInt(parts[0], 10);
    const minutes = parseInt(parts[1], 10);
    return hours * 60 + minutes;
}
