// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Дошка оголошень - Основний JavaScript файл

$(document).ready(function() {
    // Ініціалізація tooltip для всіх елементів з атрибутом title
    initializeTooltips();
    
    // Ініціалізація загальних анімацій
    initializeAnimations();
    
    // Автоматичне закриття алертів через 5 секунд
    autoCloseAlerts();
    
    // Ініціалізація прогрес-бару завантаження
    initializeLoadingIndicator();
    
    // Валідація форм в реальному часі
    initializeFormValidation();
});

// Ініціалізація tooltip
function initializeTooltips() {
    $('[data-bs-toggle="tooltip"], [title]').tooltip({
        placement: 'top',
        trigger: 'hover'
    });
}

// Загальні анімації для інтерфейсу
function initializeAnimations() {
    // Плавна поява елементів при прокрутці
    $(window).on('scroll', function() {
        $('.fade-in').each(function() {
            var elementTop = $(this).offset().top;
            var elementBottom = elementTop + $(this).outerHeight();
            var viewportTop = $(window).scrollTop();
            var viewportBottom = viewportTop + $(window).height();
            
            if (elementBottom > viewportTop && elementTop < viewportBottom) {
                $(this).addClass('visible');
            }
        });
    });
    
    // Анімація для карток при наведенні
    $(document).on('mouseenter', '.card', function() {
        $(this).addClass('shadow-lg transition-shadow');
    }).on('mouseleave', '.card', function() {
        $(this).removeClass('shadow-lg');
    });
    
    // Анімація для кнопок
    $(document).on('click', '.btn', function() {
        $(this).addClass('btn-clicked');
        setTimeout(() => {
            $(this).removeClass('btn-clicked');
        }, 150);
    });
}

// Автоматичне закриття алертів
function autoCloseAlerts() {
    $('.alert:not(.alert-permanent)').each(function() {
        var $alert = $(this);
        setTimeout(function() {
            $alert.fadeOut(500, function() {
                $(this).remove();
            });
        }, 5000);
    });
}

// Індикатор завантаження
function initializeLoadingIndicator() {
    // Показати індикатор при AJAX запитах
    $(document).ajaxStart(function() {
        showLoadingIndicator();
    }).ajaxStop(function() {
        hideLoadingIndicator();
    });
    
    // Показати індикатор при відправці форм (крім форм з data-no-loading)
    $('form:not([data-no-loading])').on('submit', function() {
        var $form = $(this);
        var $submitBtn = $form.find('button[type="submit"]');
        
        if ($submitBtn.length) {
            var originalText = $submitBtn.html();
            $submitBtn.prop('disabled', true)
                     .html('<span class="spinner-border spinner-border-sm me-2"></span>Завантаження...');
            
            // Повернути оригінальний текст через 10 секунд якщо форма не відправилась
            setTimeout(function() {
                $submitBtn.prop('disabled', false).html(originalText);
            }, 10000);
        }
    });
}

function showLoadingIndicator() {
    if ($('#loadingIndicator').length === 0) {
        $('body').append(`
            <div id="loadingIndicator" class="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center" 
                 style="background: rgba(0,0,0,0.5); z-index: 9999;">
                <div class="spinner-border text-primary" style="width: 3rem; height: 3rem;">
                    <span class="visually-hidden">Завантаження...</span>
                </div>
            </div>
        `);
    }
}

function hideLoadingIndicator() {
    $('#loadingIndicator').fadeOut(300, function() {
        $(this).remove();
    });
}

// Валідація форм в реальному часі
function initializeFormValidation() {
    // Валідація email
    $('input[type="email"]').on('blur', function() {
        var email = $(this).val();
        var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        
        if (email && !emailPattern.test(email)) {
            showFieldError($(this), 'Введіть правильний email адрес');
        } else {
            clearFieldError($(this));
        }
    });
    
    // Валідація обов'язкових полів
    $('input[required], textarea[required], select[required]').on('blur', function() {
        if (!$(this).val().trim()) {
            showFieldError($(this), 'Це поле є обов\'язковим');
        } else {
            clearFieldError($(this));
        }
    });
    
    // Валідація довжини тексту
    $('textarea[maxlength]').on('input', function() {
        var $textarea = $(this);
        var maxLength = parseInt($textarea.attr('maxlength'));
        var currentLength = $textarea.val().length;
        var remaining = maxLength - currentLength;
        
        var $counter = $textarea.siblings('.char-counter');
        if ($counter.length === 0) {
            $counter = $('<div class="char-counter form-text"></div>');
            $textarea.after($counter);
        }
        
        $counter.text(`${currentLength}/${maxLength} символів`);
        
        if (remaining < 50) {
            $counter.removeClass('text-muted').addClass('text-warning');
        } else if (remaining < 10) {
            $counter.removeClass('text-warning').addClass('text-danger');
        } else {
            $counter.removeClass('text-warning text-danger').addClass('text-muted');
        }
    });
}

function showFieldError($field, message) {
    clearFieldError($field);
    $field.addClass('is-invalid');
    $field.after(`<div class="invalid-feedback">${message}</div>`);
}

function clearFieldError($field) {
    $field.removeClass('is-invalid');
    $field.siblings('.invalid-feedback').remove();
}

// Утилітарні функції
const Utils = {
    // Показати toast повідомлення
    showToast: function(message, type = 'success', duration = 3000) {
        var toastClass = type === 'success' ? 'bg-success' : 'bg-danger';
        var iconClass = type === 'success' ? 'fa-check-circle' : 'fa-exclamation-circle';
        
        var toastHtml = `
            <div class="toast align-items-center text-white ${toastClass} border-0" role="alert">
                <div class="d-flex">
                    <div class="toast-body">
                        <i class="fas ${iconClass} me-2"></i>${message}
                    </div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `;
        
        // Створити контейнер для toast якщо його немає
        if ($('.toast-container').length === 0) {
            $('body').append('<div class="toast-container position-fixed bottom-0 end-0 p-3"></div>');
        }
        
        var $toast = $(toastHtml);
        $('.toast-container').append($toast);
        
        var toast = new bootstrap.Toast($toast[0], { delay: duration });
        toast.show();
        
        // Видалити toast після закриття
        $toast.on('hidden.bs.toast', function() {
            $(this).remove();
        });
    },
    
    // Підтвердження дії
    confirmAction: function(message, callback) {
        if (confirm(message)) {
            callback();
        }
    },
    
    // Форматування дати
    formatDate: function(date, format = 'dd.MM.yyyy') {
        var d = new Date(date);
        var day = ('0' + d.getDate()).slice(-2);
        var month = ('0' + (d.getMonth() + 1)).slice(-2);
        var year = d.getFullYear();
        
        return format.replace('dd', day).replace('MM', month).replace('yyyy', year);
    },
    
    // Обрізання тексту
    truncateText: function(text, length = 100) {
        return text.length > length ? text.substring(0, length) + '...' : text;
    },
    
    // Дебаунс функція для пошуку
    debounce: function(func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }
};

// Глобальні налаштування AJAX
$.ajaxSetup({
    beforeSend: function(xhr, settings) {
        // Додати CSRF токен до всіх AJAX запитів
        var token = $('input[name="__RequestVerificationToken"]').val();
        if (token) {
            xhr.setRequestHeader('RequestVerificationToken', token);
        }
    },
    error: function(xhr, status, error) {
        console.error('AJAX Error:', error);
        Utils.showToast('Сталася помилка при виконанні запиту', 'error');
    }
});

// Експорт функцій для глобального використання
window.Utils = Utils;
window.showLoadingIndicator = showLoadingIndicator;
window.hideLoadingIndicator = hideLoadingIndicator;
