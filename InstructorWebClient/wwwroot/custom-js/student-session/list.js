$(function () {
    $(document).off('click', '.add-student-session').on('click', '.add-student-session', fnCreate);

    $(document).off('click', '.btn-edit').on('click', '.btn-edit', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGet(id);
    });

    $(document).off('click', '.icon-delete').on('click', '.icon-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
    });
});

function fnGet(id) {
    fnHandleAjaxRequest({
        url: `/Student/GetStudentSession/${id}`,
        onSuccess: function (html) {
            $('#divCreateStudentSession').html(html);
            $('#modal-student').modal('show');
        }
    });
}

function fnCreate() {
    fnHandleAjaxRequest({
        url: '/Student/CreateStudentSession',
        onSuccess: function (html) {
            $('#divCreateStudentSession').html(html);
            $('#modal-student-session').modal('show');
        },
        onError: function () {
            fnGetNotified('error', "Failed to load form.");
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Student/DeleteStudentSession/${id}`,
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