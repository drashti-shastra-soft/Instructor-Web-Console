$(function () {
    $(document).off('click', '.add-course').on('click', '.add-course', fnCreate);

    $(document).off('click', '.btn-edit').on('click', '.btn-edit', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGet(id);
    });

    $(document).off('click', '.btn-delete').on('click', '.btn-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
    });

    $(document).off('click', '.btn-summary').on('click', '.btn-summary', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        const name = $(this).data('name');
        if (id) fnGetSummary(id, name);
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

function fnGet(id) {
    fnHandleAjaxRequest({
        url: `/Course/Get/${id}`,
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-course').modal('show');
        }
    });
}

function fnCreate() {
    fnHandleAjaxRequest({
        url: '/Course/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-course').modal('show');
        },
        onError: function () {
            fnGetNotified('error', "Failed to load form.");
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Course/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                fnGetNotified(response.type, response.msg);
                $('#frmList').submit();
            }
        },
        onError: function (errorMsg) {
            fnGetNotified('error', errorMsg || "Failed to delete user.");
        }
    });
}

