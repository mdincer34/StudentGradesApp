$(document).ready(function() {
    function loadPartialView(url, showModal = true) {
        $.get(url, function(data) {
            $('#partialViewModal .modal-content').html(data);
            if (showModal) {
                $('#partialViewModal').modal('show');
            }
        });
    }

    function ajaxSubmit(form, successCallback) {
        $.ajax({
            url: form.attr('action'),
            type: form.attr('method'),
            data: form.serialize(),
            success: function(result) {
                if (result.success) {
                    successCallback(result);
                } else {
                    alert('İşlem başarısız: ' + result.message);
                }
            },
            error: function(xhr, status, error) {
                console.error("AJAX error:", status, error);
                alert("Bir hata oluştu: " + error);
            }
        });
    }

    $(document).on('submit', '#editGradeWeightForm', function(e) {
        e.preventDefault();
        ajaxSubmit($(this), function(result) {
            if (result.success) {
                alert(result.message);
                $('#partialViewModal').modal('hide');
                location.reload();
            } else {
                alert('İşlem başarısız: ' + result.message);
            }
        });
    });

    window.initializeGradeWeightEdit = function(otherGradeTypesWeight, originalWeight) {
        var weightInput = $("#weightInput");
        var weightWarning = $("#weightWarning");
        var submitButton = $("#submitButton");

        weightInput.on("input", function() {
            var newWeight = parseInt($(this).val());
            var totalWeight = newWeight + otherGradeTypesWeight - originalWeight;

            if (isNaN(newWeight) || newWeight < 0 || newWeight > 100 || totalWeight > 100) {
                weightWarning.show();
                submitButton.prop("disabled", true);
            } else {
                weightWarning.hide();
                submitButton.prop("disabled", false);
            }
        });
    };

    window.editGradeWeight = function(id) {
        loadPartialView('/GradeType/EditGradeWeight/' + id, function() {
            var otherGradeTypesWeight = parseInt($("#otherGradeTypesWeight").val());
            var originalWeight = parseInt($("#originalWeight").val());
            initializeGradeWeightEdit(otherGradeTypesWeight, originalWeight);
        });
    };


    $(document).on('submit', '#addOrUpdateGradeForm', function(e) {
        e.preventDefault();
        ajaxSubmit($(this), function(result) {
            alert(result.message);
            $('#partialViewModal').modal('hide');
            location.reload();
        });
    });

    $(document).on('submit', '#editStudentForm', function(e) {
        e.preventDefault();
        ajaxSubmit($(this), function(result) {
            alert(result.message);
            $('#partialViewModal').modal('hide');
            location.reload();
        });
    });

    $(document).on('submit', '#createStudentForm', function(e) {
        e.preventDefault();
        ajaxSubmit($(this), function(result) {
            alert(result.message);
            $('#partialViewModal').modal('hide');
            location.reload();
        });
    });

    window.loadStudentAction = function(studentId, action) {
        loadPartialView('/Student/' + action + '/' + studentId);
    };

    window.editStudent = function(id) {
        loadPartialView('/Student/Edit/' + id);
    };

    window.deleteStudent = function(id) {
        if (confirm('Bu öğrenciyi silmek istediğinizden emin misiniz?')) {
            $.post('/Student/Delete/' + id, { __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val() }, function(result) {
                if (result.success) {
                    alert(result.message);
                    location.reload();
                } else {
                    alert('İşlem başarısız: ' + result.message);
                }
            });
        }
    };

    window.deleteGrade = function(studentId, gradeTypeId) {
        if (confirm('Bu notu silmek istediğinizden emin misiniz?')) {
            $.ajax({
                url: '/Grade/DeleteGrade',
                type: 'POST',
                data: {
                    studentId: studentId,
                    gradeTypeId: gradeTypeId,
                    __RequestVerificationToken: $('input[name="__RequestVerificationToken"]').val()
                },
                success: function(result) {
                    if (result.success) {
                        alert(result.message);
                        location.reload();
                    } else {
                        alert('İşlem başarısız: ' + result.message);
                    }
                },
                error: function(xhr, status, error) {
                    console.error("AJAX error:", status, error);
                    alert("Bir hata oluştu: " + error);
                }
            });
        }
    };

    window.createStudent = function() {
        loadPartialView('/Student/Create');
    };

    window.closeSemester = function() {
        if (confirm('Dönemi kapatmak istediğinizden emin misiniz? Bu işlem geri alınamaz.')) {
            $.ajax({
                url: '/Semester/CloseSemester',
                type: 'POST',
                headers: {
                    "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val()
                },
                success: function(result) {
                    if (result.success) {
                        alert(result.message);
                        location.reload();
                    } else {
                        alert('İşlem başarısız: ' + result.message);
                    }
                },
                error: function(xhr, status, error) {
                    console.error("AJAX error:", status, error);
                    alert("Bir hata oluştu: " + error);
                }
            });
        }
    };

    window.loadPartialView = loadPartialView;

    $(document).on('submit', '#passingGradeForm', function(e) {
        e.preventDefault();
        ajaxSubmit($(this), function(result) {
            if (result.success) {
                $('#passingGradeDropdown').text('Geçme Notu: ' + result.newPassingGrade);
                $('#partialViewModal').modal('hide');
                // Geçme notunu güncelle ve durum sütununu yeniden hesapla
                updatePassingGradeAndStatus(result.newPassingGrade);
            } else {
                alert('İşlem başarısız: ' + result.message);
            }
        });
    });

    function updatePassingGradeAndStatus(newPassingGrade) {
        $('table tbody tr').each(function() {
            var row = $(this);
            var ortalama = parseFloat(row.find('td:eq(5)').text());
            var durumCell = row.find('td:eq(6)');
            if (!isNaN(ortalama)) {
                var yeniDurum = ortalama >= newPassingGrade ? "Geçti" : "Kaldı";
                durumCell.text(yeniDurum);
            }
        });
    }
});