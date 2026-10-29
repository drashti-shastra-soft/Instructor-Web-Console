$(function () {
    $(document).off('click', '.add-student-course').on('click', '.add-student-course', fnCreateCourse);

    $(document).off('click', '.edit-btn').on('click', '.edit-btn', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGetCourse(id);
    });

    $(document).off('click', '.delete-btn').on('click', '.delete-btn', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDeleteCourse(id);
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



function fnGetCourse(id) {
    fnHandleAjaxRequest({
        url: `/StudentCourse/Get/${id}`,
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-student-course').modal('show');
        },
        onError: function () {
            fnGetNotified('error', "Failed to load form.");
        }
    });
}

function fnCreateCourse() {
    fnHandleAjaxRequest({
        url: '/StudentCourse/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-student-course').modal('show');
        },
        onError: function () {
            fnGetNotified('error', "Failed to load form.");
        }
    });
}

function fnDeleteCourse(id) {
    fnHandleAjaxRequest({
        url: `/StudentCourse/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                fnGetNotified(response.type, response.msg);
                $('#frmList').submit();
            }
        },
        onError: function () {
            fnGetNotified('error', "Failed to delete.");
        }
    });
}