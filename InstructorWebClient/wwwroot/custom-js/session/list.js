$(function () {
    $(document).off('click', '.add-session').on('click', '.add-session', fnCreate);

    $(document).off('click', '.btn-edit').on('click', '.btn-edit', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        const classType = $(this).data('class-type');
        if (id) fnGet(id, classType);
    });

    $(document).off('click', '.icon-delete').on('click', '.icon-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
    });

    $(document).off('click', '.btn-summary').on('click', '.btn-summary', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGet(id);
    });

    // Reset pagination and submit the form on Enter key in search fields
    $(document).off('keyup', '.form-search-input').on('keyup', '.form-search-input', function (e) {
        if (e.which === 13) {
            e.preventDefault();
            resetPagination();
            $('#frmList').trigger('submit');
        }
    });

    // Submit form when page size filter changes
    $(document).off('change', '.search-pagination').on('change', '.search-pagination', function () {
        $('#frmList').trigger('submit');
    });

    $(document).off('click', '.navigation').on('click', '.navigation', function (e) {
        e.preventDefault();
        var direction = $(this).data('direction').trim();
        if (direction != null && direction != undefined && direction != '') {
            triggerPageNavigation(direction);
        }
        $('#frmList').trigger('submit');
    });
});

function fnGet(id, classType) {
    fnHandleAjaxRequest({
        url: `/User/Get?id=${id}&classType=${classType}`,
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-session').modal('show');
        }
    });
}

function fnCreate() {
    fnHandleAjaxRequest({
        url: '/Session/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-session').modal('show');
        },
        onError: function () {
            fnGetNotified('error', "Failed to load form.");
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Session/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                fnGetNotified(response.type, response.msg);
                $('#frmList').submit();
            }
        },
        onError: function () {
            fnGetNotified('error', "Failed to delete session.");
        }
    });
}

$(document).ready(function () {    
    $('.form-search-input').each(function (index, element) {        
        const $input = $(element);
        if ($input.val().trim() === '01/01/0001' || $input.val().trim() === '1/1/0001' || $input.val().trim() === '01-01-0001' || $input.val().trim() === '1-1-0001') {            
            $input.val('');
        } else if ($input.val().trim() === '12:00 AM') {
            $input.val('');
        } else if ($input.val().trim() === '0') {
            $input.val('');
        }
    });
});