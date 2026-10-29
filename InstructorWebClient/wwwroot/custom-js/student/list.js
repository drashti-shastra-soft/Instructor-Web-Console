$(function () {
    $(document).off('click', '.add-student').on('click', '.add-student', fnCreate);

    $(document).off('click', '.edit-btn').on('click', '.edit-btn', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnGet(id);
    });


    $(document).off('click', '.icon-studentcourse').on('click', '.icon-studentcourse', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) {
            window.location.href = '/StudentCourse/Index/' + id;
        }
    });

    $(document).off('click', '.icon-address').on('click', '.icon-address', function (e) {
        e.preventDefault();
        const studentId = $(this).data('id');
        if (studentId) fnGetAddress(studentId);
    });


    $(document).off('click', '.icon-delete').on('click', '.icon-delete', function (e) {
        e.preventDefault();
        const id = $(this).data('id');
        if (id) fnDelete(id);
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

    $(document).off('click', '.toggle-child').on('click', '.toggle-child', function (e) {
        const id = $(this).data('id');
        const $childRow = $('#child_' + id);
        const $icon = $(this).find('i');
        const $content = $('#childContent_' + id);

        const isVisible = $childRow.is(':visible');

        // Close all other child rows
        $('.child-row').hide();
        $('.toggle-child i').removeClass('fa-minus').addClass('fa-plus');
        $('.child-row div[id^="childContent_"]').empty();

        if (!isVisible) {
            $childRow.show();
            $icon.removeClass('fa-plus').addClass('fa-minus');
            fnGetStudentSessions(id, $content); // Load content only when opening
        }
    });
});


function fnGetCourse(id) {
    debugger;
    fnHandleAjaxRequest({
        url: `/StudentCourse/GetMeta/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response.success && response.redirectUrl) {
                window.location.href = response.redirectUrl;
            }
        },
        onError: function () {
            fnGetNotified('error', "Failed to load form.");
        }
    });
}

function fnGet(id) {
    fnHandleAjaxRequest({
        url: `/Student/Get/${id}`,
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-student').modal('show');
        },
        onError: function () {
            fnGetNotified('error', "Failed to load form.");
        }
    });
}


function fnGetAddress(id) {
    $('#loading-element').show();
    $.ajax({
        url: `/Student/GetStudentAddress/${id}`,
        type: "Get",
        dataType: "html",
        success: function (responseData) {
            $("#divAddress").empty().html(responseData);
            $("#modal-address").modal('show');
            $('#loading-element').hide();
        },
        error: function (xhr, status, error) {
            $('#loading-element').hide();
        }
    });
}

function fnGetStudentSessions(id, $content) {
    fnHandleAjaxRequest({
        url: `/Student/GetStudentCourses/${id}`,
        onSuccess: function (html) {
            $content.html(html);
        },
        onError: function () {
            $content.html('<div class="text-danger">Unable to load session data.</div>');
        }
    });
}

function fnCreate() {
    fnHandleAjaxRequest({
        url: '/Student/Create',
        onSuccess: function (html) {
            $('#divCreate').html(html);
            $('#modal-student').modal('show');
        },
        onError: function () {            
            fnGetNotified('error', "Failed to load form.");
        }
    });
}

function fnDelete(id) {
    fnHandleAjaxRequest({
        url: `/Student/Delete/${id}`,
        dataType: 'json',
        onSuccess: function (response) {
            if (response) {
                fnGetNotified(response.type, response.msg);
                $('#frmList').submit();
            }
        },
        onError: function () {            
            fnGetNotified('error', "Failed to delete student.");
        }
    });
}

function applyRowStriping() {
    $('tr[id^="row_"] td').css('background-color', ''); // reset
    $('tr[id^="row_"]').each(function (index) {
        const bgColor = index % 2 === 0 ? '#f2f2f2' : '#ffffff';
        $(this).find('td').css('background-color', bgColor);
    });
}

$(document).ready(function () {
    applyRowStriping();
});

