const main = document.querySelector("main");

main.addEventListener("dragenter", (e) => {
    if (e.target.classList.contains("list-group")) {
        e.target.classList.add("drop");
    }
});

main.addEventListener("dragleave", (e) => {
    if (e.target.classList.contains("drop")) {
        e.target.classList.remove("drop");
    }
});

main.addEventListener("dragstart", (e) => {
    if (e.target.classList.contains("list-group-item")) {
        e.dataTransfer.setData("text/plain", e.target.dataset.id);
    }
});

let elemBelow = "";

main.addEventListener("dragover", (e) => {
    e.preventDefault();

    elemBelow = e.target;
});

main.addEventListener("drop", (e) => {
    const todo = main.querySelector(
        `[data-id="${e.dataTransfer.getData("text/plain")}"]`
    );

    if (elemBelow === todo) {
        return;
    }

    if (elemBelow.tagName === "P" || elemBelow.tagName === "BUTTON") {
        elemBelow = elemBelow.parentElement;
    }

    if (elemBelow.classList.contains("list-group-item")) {
        const center =
            elemBelow.getBoundingClientRect().y +
            elemBelow.getBoundingClientRect().height / 2;

        if (e.clientY > center) {
            if (elemBelow.nextElementSibling !== null) {
                elemBelow = elemBelow.nextElementSibling;
            } else {
                return;
            }
        }

        elemBelow.parentElement.insertBefore(todo, elemBelow);
        todo.className = elemBelow.className;
    }

    if (e.target.classList.contains("list-group")) {
        e.target.append(todo);

        if (e.target.classList.contains("drop")) {
            e.target.classList.remove("drop");
        }

        const { name } = e.target.dataset;

        if (name === "disadvantage-list") {
            if (todo.classList.contains("in-advantage")) {
                todo.classList.remove("in-advantage");
            }
            todo.classList.add("in-disadvantage");
        } else if (name === "advantage-list") {
            if (todo.classList.contains("in-disadvantage")) {
                todo.classList.remove("in-disadvantage");
            }
            todo.classList.add("in-advantage");
        } else {
            todo.className = "list-group-item";
        }
    }
});