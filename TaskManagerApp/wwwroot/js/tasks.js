function ChangeStatusAndRemoveCard(id, oldStatus, statusToMove) {
    $.ajax({
        url: `/api/Task/status/${id}`,
        type: 'PUT',
        contentType: "application/json",
        data: JSON.stringify({
            "Status": statusToMove
        }),
        success: function (response) {
            const changeStatusEvent = new Event("OnChangeStatusEvent", {
                bubbles: true,
                detail: {
                    id: id,
                    oldStatus: oldStatus,
                    newStatus: statusToMove
                }
            })

            document.getElementById("task-" + id + "-" + oldStatus).dispatchEvent(changeStatusEvent)
            removeCard(id, oldStatus)
            createNewCard(id, "column-" + statusToMove.toLowerCase(), statusToMove)
        }
    })
}

function removeCard(id, status) {
    let statusFormat = status
    document.getElementById("task-" + id + "-" + statusFormat).classList.remove("makeVisible")
    document.getElementById("task-" + id + "-" + statusFormat).classList.add("makeInvisible")
    setTimeout(() => {
        document.getElementById("task-" + id + "-" + statusFormat).remove()
    }, 1000)
}

function CreateNewCardOnCorrectStatusBoard(id, status) {
    createNewCard(id, "column-" + status.toLowerCase(), status)
}

function createNewCard(id, classToAddHtmlContent, status) {
    $.ajax({
        url: `/api/Task/${id}`,
        success: function (response) {
            $("." + classToAddHtmlContent).append(response.result)

            document.getElementById("task-" + id + "-" + status).classList.add("makeVisible")
        }
    })
}


$(".bi-pencil").hover(function () {
    let $this = $(this)
    this.classList.remove("bi-pencil")
    this.classList.add("bi-pencil-fill")
}, function () {
    let $this = $(this)
    this.classList.remove("bi-pencil-fill")
    this.classList.add("bi-pencil")
});

$(".bi-trash").hover(function () {
    let $this = $(this)
    this.classList.remove("bi-trash")
    this.classList.add("bi-trash-fill")
}, function () {
    let $this = $(this)
    this.classList.remove("bi-trash-fill")
    this.classList.add("bi-trash")
});


function deleteTaskModal(id, status) {
    $("#delete-task-modal").modal("toggle");
    $("#input-delete-id").val(id);
    $("#input-delete-status").val(status)
}

function deleteTask() {
    let taskId = $("#input-delete-id").val()
    let taskStatus = $("#input-delete-status").val()
    $.ajax({
        url: `api/Task/${taskId}`,
        type: "DELETE",
        success: function (response) {
            removeCard(taskId, taskStatus)
            $("#delete-task-modal").modal("toggle");
        }
    })
}

function createTaskModal() {
    $("#create-task-modal").modal("toggle")
}

function createTask() {
    let title = $("#create-task-title-input").val()
    let description = $("#create-task-content-input").val()
    let status = $("#create-task-status").find(":selected").val()

    let valueStatusMap = {
        1: "Todo",
        2: "Doing",
        3: "Done",
        4: "Archived",
    }

    $.ajax({
        url: "api/Task",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify({
            "taskDto": {
                "Title": title.toString(),
                "Description": description.toString(),
                "Status": valueStatusMap[status].toString()
            },
            "Title": title.toString(),
            "Description": description.toString(),
            "Status": valueStatusMap[status].toString()
        }),

        success(response) {
            createTaskModal();
            CreateNewCardOnCorrectStatusBoard(response.id, valueStatusMap[status])
        }


    })
}
