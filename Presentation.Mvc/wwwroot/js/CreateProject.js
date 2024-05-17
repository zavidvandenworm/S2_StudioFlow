const formProject = document.getElementById("create-project-form");
const tagAddInput = document.getElementById("tag-add-input");
const tagList = document.getElementById("tag-list");
const submitButton = document.getElementById("submit-create-project");

formProject.onsubmit = (ev) => {
    
}

submitButton.onclick = () => {
    formProject.requestSubmit();
}

tagAddInput.onkeydown = (key) => {
    if (key.key !== "Enter"){
        return;
    }
    if (tagAddInput.value === ""){
        return;
    }
    for(let i = 0; i < tagList.children.length; i++){
        let elem = tagList.children.item(i);
        if (elem.innerText === tagAddInput.value){
            return;
        }
    }
    
    let newChild = document.createElement("span");
    newChild.innerText = tagAddInput.value;
    newChild.onclick = () => {
        tagList.removeChild(newChild);
    };
    
    tagAddInput.value = "";

    tagList.appendChild(newChild);
}