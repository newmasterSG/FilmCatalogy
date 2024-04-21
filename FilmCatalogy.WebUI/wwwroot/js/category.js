class CategorySelector {
    constructor(elementId) {
        this.elementId = elementId;
        this.categories = [];

        this.selectElement = document.getElementById(elementId);

        this.updateCategories = this.updateCategories.bind(this);

        this.selectElement.addEventListener('change', this.updateCategories);
    }

    updateCategories() {
        var d = Array.from(this.checkboxContainer.querySelectorAll('input[type="checkbox"]:checked')).map(checkbox => parseInt(checkbox.value));
        this.categories = d;
    }

    addCategory(categoryId) {
        const checkbox = this.checkboxContainer.querySelector(`input[type="checkbox"][value="${categoryId}"]`);
        if (checkbox) {
            checkbox.checked = true;
            this.updateCategories();
        }
    }

    removeCategory(categoryId) {
        const checkbox = this.checkboxContainer.querySelector(`input[type="checkbox"][value="${categoryId}"]`);
        if (checkbox) {
            checkbox.checked = false;
            this.updateCategories();
        }
    }
}

function fetchCategories(filmId) {
    Promise.all([
        fetch('/api/categoryApi/all').then(response => response.json()),
        fetch(`/api/categoryApi/${filmId}`).then(response => response.json())
    ]).then(([allCategories, filmCategories]) => {
        const checkboxList = document.getElementById('categoryCheckboxList');
        allCategories.forEach(category => {
            const checkbox = document.createElement('input');
            checkbox.type = 'checkbox';
            checkbox.name = 'CategoryIds';
            checkbox.value = category.id;

            const isChecked = filmCategories.some(filmCategory => filmCategory.id === category.id);
            checkbox.checked = isChecked;

            const label = document.createElement('label');
            label.textContent = category.name;

            const lineBreak = document.createElement('br');

            checkboxList.appendChild(checkbox);
            checkboxList.appendChild(label);
            checkboxList.appendChild(lineBreak);
        });
    }).catch(error => console.error('Error:', error));
}

function updateFilmCategories(filmId) {
    const selectedCategories = categorySelector.categories;

    fetch(`/api/categoryApi/${filmId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(selectedCategories)
    })
        .then(response => response.json())
        .then(() => console.log('Categories updated successfully'))
        .catch(error => console.error('Error:', error));
}