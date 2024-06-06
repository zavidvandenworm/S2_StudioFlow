let nameHtml = document.getElementById("task-name");
let descriptionHtml = document.getElementById("task-description");
let btn = document.getElementById("completion_button");

let successToast = Toastify({
    text: "Rewrote task data!",
    duration: 3000,
    gravity: "bottom", // `top` or `bottom`
    position: "right", // `left`, `center` or `right`
    stopOnFocus: true, // Prevents dismissing of toast on hover
    style: {
        background: "linear-gradient(to right, #00b09b, #96c93d)",
    },
});

let failToast = Toastify({
    text: "Failed to rewrite task data.",
    duration: 3000,
    gravity: "bottom", // `top` or `bottom`
    position: "right", // `left`, `center` or `right`
    stopOnFocus: true, // Prevents dismissing of toast on hover
    style: {
        background: "rgb(255,100,100)",
    },
});

btn.onclick = () => {
    AiFill(nameHtml.value, descriptionHtml.value)
}

async function AiFill(name, description){
    fetch("/completions/task?" + new URLSearchParams({
        title: name,
        description: description,
        tags: "tag"
    }))
        .then((response) => {
            response.text().then((text) => {
                let js = JSON.parse(text);
                nameHtml.value = js["title"];
                descriptionHtml.value = js["description"];
                successToast.showToast();
            }).catch(() => {
                failToast.showToast();
            })
        })
}