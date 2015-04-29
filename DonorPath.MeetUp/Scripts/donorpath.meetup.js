var demoTimes;

function FetchDemoTimes() {

    var localNow = moment();
    var utcNow = moment.utc();

    var localHour = localNow.hours();
    var utcHour = utcNow.hours();

    if (localHour < utcHour)
        $('#timezone').text(localHour - utcHour);
    else
        $('#timezone').text("+" + (localHour - utcHour));

    $('#TimezoneOffset').val(moment().utcOffset());
    var rootUri = location.toString();
    if (location.pathname != '/')
        rootUri = rootUri.replace(location.pathname, '/');

    var appointmentUri = rootUri.split('?')[0] + 'home/appointments';
    $.getJSON( appointmentUri)
      .done(function (data) {
          demoTimes = $.map(data, function (e) { return moment.utc(e); });
          BindDemoTimes();
          EnableForm();
      });
}

function EnableForm()
{
    $('#MeetupLoad').hide();
    $('#appointmentSubmit').attr('disabled', false);
    $('#AppointmentTime').attr('disabled', false);
    $('#Email').attr('disabled', false);
}

function DisableForm()
{
    $('#submitSuccess').hide();
    $('#submitFailure').hide();
    BindDemoTimes();
    $('#MeetupLoad').show();
    $('#appointmentSubmit').attr('disabled', true);
    $('#AppointmentTime').attr('disabled', true);
    $('#Email').attr('disabled', true);
}


function BindDemoTimes() {
    Date.parseDate = function (input, format) {
        return moment(input, format).toDate();
    };
    Date.prototype.dateFormat = function (format) {
        return moment(this).format(format);
    };
    if ($('#AppointmentTime').length > 0) {
        $("#AppointmentTime").datetimepicker({
            onGenerate: function (ct) {
                $('.xdsoft_date').each(function () {
                    var calDate = (parseInt($(this).attr('data-month')) + 1) + '/' + $(this).attr('data-date') + '/' + $(this).attr('data-year');
                    var result = $.grep(demoTimes, function (e) {
                        return e.local().format('l') === calDate;
                    });
                    if (result.length === 0) {
                        $(this).addClass('xdsoft_disabled');
                    }
                });
            },
            timepicker: false,
            formatTime: 'h:mm a',
            format: 'MM/DD/YYYY h:mm a',
            formatDate: 'MM/DD/YYYY',
            onChangeDateTime: function (c) {
                if (c) {
                    var formattedCurrentDate = moment(c.toISOString()).format("l"),
                        times = $.map(demoTimes, function (e, i) {
                            if (e.local().format('l') === formattedCurrentDate) return e.local().format('H:mm a');
                        });
                    if (times.length > 0) {
                        this.setOptions({
                            timepicker: true,
                            allowTimes: times
                        });
                    }
                }
            }
        });
    }
}

function initGenericDateTimePicker() {
    if ($('.datetimepicker').length > 0) {
        $(".datetimepicker").datetimepicker({
            formatTime: 'h:mm a',
            format: 'MM/DD/YYYY h:mm a',
            formatDate: 'MM/DD/YYYY',
            step: 15
        });
    }
}

function OnFailure(jqXHR, textStatus, errorThrown)
{
    $('#submitFailure').text(errorThrown);
    $('#submitFailure').show();
    return false;
}

function OnSuccess(data, textStatus, jqXHR) {
    $('#submitSuccess').show();
    return false;
}

function OnBegin(jqXHR, settings)
{
    DisableForm();
}
    
function OnComplete(jqXHR, textStatus)
{
    EnableForm()
}