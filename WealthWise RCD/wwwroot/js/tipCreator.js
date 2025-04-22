document.addEventListener("DOMContentLoaded", function () {
    // SAVING FUNCTIONALITY
    const options = {
        debug: 'info',
        modules: {
            toolbar: [
                [{ header: [1, 2, false] }],
                ['bold', 'italic', 'underline'],
                [{ list: 'ordered' }, { list: 'bullet' }],
                ['clean'],
            ],
            history: true,
        },
        placeholder: 'Start writing here...',
        theme: 'snow'
    };

    const quill = new Quill('#editor', options);

    // Set the character limit
    const MAX_CONTENT_LENGTH = 200;

    quill.on('text-change', function (delta, oldDelta, source) {
        let text = quill.getText().trim();
        let words = text.split(/\s+/).filter(w => w.length > 0);

        if (words.length > MAX_CONTENT_LENGTH) {
            quill.history.undo(); // Undo the last change that made the word limit over 200.
            // KNOWN RISK: If user pastes over 200 words of text it will undo to nothing.
        }

        // show character count
        const charCount = document.getElementById('charCount');
        if (charCount) {
            charCount.textContent = `Word Limit: ${Math.min(words.length, MAX_CONTENT_LENGTH)} / ${MAX_CONTENT_LENGTH}`;
        }
    });

    // style the toolbar
    setTimeout(() => {
        document.querySelector('.ql-toolbar').style.backgroundColor = '#C4C4A1';
    }, 100);

    // Add Save button
    const toolbar = quill.getModule('toolbar');
    const saveButton = document.createElement('button');
    saveButton.innerHTML = '<i class="fa-regular fa-floppy-disk" style="color: #3F3F2F;"></i>';
    saveButton.className = 'ql-save';
    saveButton.title = 'Save Post';
    saveButton.id = 'saveBtn';
    toolbar.container.appendChild(saveButton);

    saveButton.addEventListener('click', function () {
        const title = document.getElementById('blogTitle').value.trim();
        const topic = document.getElementById('blogTopic').value.trim();
        const content = quill.root.innerHTML.trim();
        const plainText = quill.getText().trim();

        clearErrors();

        if (!missingField(title, topic, content, plainText)) {
            return;
        }

        const blogData = {
            Title: title,
            Topic: topic,
            Content: content
        };

        fetch('/Advisor/LearningHub/SaveTempBlog', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(blogData)
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(text => { throw new Error(text); });
                }
                return response.text();
            })
            .then(data => {
                alert('Tip saved successfully.');
                const lastSavedTime = document.getElementById('lastSaved');
                if (lastSavedTime) {
                    const now = new Date().toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
                    lastSavedTime.textContent = `Last saved: ${now}`;
                }
            })
            .catch(error => {
                console.error('Error:', error);
                alert("Failed to save tip draft. Check the console for details.");
            });
    });

    document.getElementById('postButton').addEventListener('click', function () {
        const title = document.getElementById('blogTitle').value.trim();
        const topic = document.getElementById('blogTopic').value.trim();
        const content = quill.root.innerHTML.trim();
        const plainText = quill.getText().trim();

        clearErrors();

        if (!missingField(title, topic, content, plainText)) {
            return;
        }

        const confirmpost = confirm("Are you sure you would like to post?");
        if (!confirmpost) return;

        const blogData = {
            Title: title,
            Topic: topic,
            Content: content
        };

        fetch('/Advisor/LearningHub/PostBlog', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(blogData)
        })
        .then(response => {
            if (!response.ok) {
                return response.text().then(text => { throw new Error(text); });
            }
            return response.text();
        })
        .then(data => {
            alert('Tip posted successfully.');
            window.location.href = '/Advisor/LearningHub/TipsAndTricks';
        })
        .catch((error) => {
            console.error('Error:', error);
            alert("Failed to post tip. Please try again.");
        });
    });

    // Title Character Limit
    const MAX_TITLE_LENGTH = 70;
    const titleInput = document.getElementById('blogTitle');

    titleInput.addEventListener('input', function () {
        if (titleInput.value.length > MAX_TITLE_LENGTH) {
            titleInput.value = titleInput.value.substring(0, MAX_TITLE_LENGTH);
        }
    })
});

function missingField(title, topic, content, plainText) {
    const titleError = document.getElementById('missingTitle');
    const topicError = document.getElementById('missingTopic');
    const contentError = document.getElementById('missingContent');

    let isValid = true;

    if (!title) {
        titleError.textContent = "Title is required.";
        titleError.style.visibility = "visible";
        isValid = false;
    }
    if (!topic) {
        topicError.textContent = "Topic is required.";
        topicError.style.visibility = "visible";
        isValid = false;
    }
    if (!plainText || plainText === "" || plainText.length > 200) {
        contentError.textContent = "Content must be between 1 and 200 characters.";
        contentError.style.visibility = "visible";
        isValid = false;
    }

    return isValid;
}

function clearErrors() {
    document.getElementById('missingTitle').style.visibility = 'hidden';
    document.getElementById('missingTopic').style.visibility = 'hidden';
    document.getElementById('missingContent').style.visibility = 'hidden';
}
