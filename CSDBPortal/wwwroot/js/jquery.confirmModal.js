/*!
 * jQuery.confirmModal v1.1 (Bootstrap 5 compatible)
 * Copyright (c) 2018 Trim C.
 * Released under the MIT license
 */
(function ($) {
    $defaultsConfirmModal = {};

    jQuery.confirmModal = function (message, options, callback) {
        var targetElement = document.activeElement;

        // Support 2-arg form: confirmModal(message, callback)
        if (typeof options === 'function') {
            callback = options;
            options  = {};
        }

        var settings = $.extend({}, $defaultsConfirmModal, options);

        var messageHeader     = settings.messageHeader  || 'Confirm';
        var confirmButton     = settings.confirmButton  || 'OK';
        var cancelButton      = settings.cancelButton   || 'Cancel';
        var modalVerticalCenter = settings.modalVerticalCenter ? 'modal-dialog-centered' : '';
        var fadeAnimation     = settings.fadeAnimation  ? 'fade' : '';
        var modalBoxWidth     = settings.modalBoxWidth  || '';

        var maxWidthStyle = modalBoxWidth ? 'max-width:' + modalBoxWidth + ';' : '';

        var html =
            '<div class="modal ' + fadeAnimation + ' modalConfirm" tabindex="-1" aria-modal="true" role="dialog" style="z-index:5000;">' +
                '<div class="modal-dialog modal-dialog-centered ' + modalVerticalCenter + '" style="' + maxWidthStyle + '">' +
                    '<div class="modal-content">' +
                        '<div class="modal-header" style="display:flex;align-items:center;justify-content:space-between;">' +
                            '<h6 class="modal-title">' + messageHeader + '</h6>' +
                            '<button type="button" class="btn-close confirmDismiss" aria-label="Close"></button>' +
                        '</div>' +
                        '<div class="modal-body" style="font-size:0.9rem;">' + message + '</div>' +
                        '<div class="modal-footer">' +
                            '<button type="button" class="confirmButton btn btn-primary btn-sm" style="min-width:66px;">' + confirmButton + '</button>' +
                            '<button type="button" class="confirmDismiss btn btn-secondary btn-sm" style="min-width:66px;">' + cancelButton + '</button>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>';

        // Remove any leftover modal before creating a new one
        var existing = document.querySelector('body > .modalConfirm');
        if (existing) {
            var prev = bootstrap.Modal.getInstance(existing);
            if (prev) prev.dispose();
            $(existing).remove();
        }

        $('body').prepend(html);
        var modalEl = document.querySelector('body > .modalConfirm');
        var modal   = new bootstrap.Modal(modalEl);

        $(modalEl).on('hidden.bs.modal', function () {
            modal.dispose();
            $(modalEl).remove();
        });

        $('.confirmButton', modalEl).on('click', function (e) {
            e.preventDefault();
            modal.hide();
            if (typeof callback === 'function') {
                callback(targetElement);
            }
        });

        $('.confirmDismiss', modalEl).on('click', function () {
            modal.hide();
        });

        modal.show();
    };
}(jQuery));
