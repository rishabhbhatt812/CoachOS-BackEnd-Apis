# CoachOS.Api
## Version: 1.0

### Available authorizations
#### Bearer (API Key Authentication)
JWT Authorization header using the Bearer scheme.  
**Name:** Authorization  
**In:** header  

---
## Attendance

### [GET] /api/admin/Attendance/sessions
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Attendance/sessions
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateAttendanceSessionRequest](#createattendancesessionrequest-schema)<br>**text/json**: [CreateAttendanceSessionRequest](#createattendancesessionrequest-schema)<br>**application/*+json**: [CreateAttendanceSessionRequest](#createattendancesessionrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/admin/Attendance/sessions/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Auth

### [POST] /api/Auth/register-institute
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [RegisterInstituteRequest](#registerinstituterequest-schema)<br>**text/json**: [RegisterInstituteRequest](#registerinstituterequest-schema)<br>**application/*+json**: [RegisterInstituteRequest](#registerinstituterequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/Auth/login
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [LoginRequest](#loginrequest-schema)<br>**text/json**: [LoginRequest](#loginrequest-schema)<br>**application/*+json**: [LoginRequest](#loginrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/Auth/create-user
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateUserRequest](#createuserrequest-schema)<br>**text/json**: [CreateUserRequest](#createuserrequest-schema)<br>**application/*+json**: [CreateUserRequest](#createuserrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Batches

### [GET] /api/admin/Batches
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Batches
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateBatchRequest](#createbatchrequest-schema)<br>**text/json**: [CreateBatchRequest](#createbatchrequest-schema)<br>**application/*+json**: [CreateBatchRequest](#createbatchrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/admin/Batches/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [PUT] /api/admin/Batches/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [UpdateBatchRequest](#updatebatchrequest-schema)<br>**text/json**: [UpdateBatchRequest](#updatebatchrequest-schema)<br>**application/*+json**: [UpdateBatchRequest](#updatebatchrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/admin/Batches/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Courses

### [GET] /api/admin/Courses
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Courses
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateCourseRequest](#createcourserequest-schema)<br>**text/json**: [CreateCourseRequest](#createcourserequest-schema)<br>**application/*+json**: [CreateCourseRequest](#createcourserequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/admin/Courses/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [PUT] /api/admin/Courses/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [UpdateCourseRequest](#updatecourserequest-schema)<br>**text/json**: [UpdateCourseRequest](#updatecourserequest-schema)<br>**application/*+json**: [UpdateCourseRequest](#updatecourserequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/admin/Courses/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Crm

### [GET] /api/admin/Crm/enquiries
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Crm/enquiries
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateEnquiryRequest](#createenquiryrequest-schema)<br>**text/json**: [CreateEnquiryRequest](#createenquiryrequest-schema)<br>**application/*+json**: [CreateEnquiryRequest](#createenquiryrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/admin/Crm/enquiries/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [PUT] /api/admin/Crm/enquiries/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [UpdateEnquiryRequest](#updateenquiryrequest-schema)<br>**text/json**: [UpdateEnquiryRequest](#updateenquiryrequest-schema)<br>**application/*+json**: [UpdateEnquiryRequest](#updateenquiryrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/admin/Crm/enquiries/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Crm/followups
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: <br>**text/json**: <br>**application/*+json**: <br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Crm/democlasses
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: <br>**text/json**: <br>**application/*+json**: <br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Crm/import
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/admin/Crm/export
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Dashboard

### [GET] /api/admin/Dashboard/metrics
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Fees

### [GET] /api/admin/Fees/plans
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Fees/plans
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateFeePlanRequest](#createfeeplanrequest-schema)<br>**text/json**: [CreateFeePlanRequest](#createfeeplanrequest-schema)<br>**application/*+json**: [CreateFeePlanRequest](#createfeeplanrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/admin/Fees/plans/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/admin/Fees/plans/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Notices

### [GET] /api/admin/Notices
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Notices
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateNoticeRequest](#createnoticerequest-schema)<br>**text/json**: [CreateNoticeRequest](#createnoticerequest-schema)<br>**application/*+json**: [CreateNoticeRequest](#createnoticerequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/admin/Notices/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## StudentPortal

### [GET] /api/student/portal/dashboard
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/student/portal/courses
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/student/portal/fees
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/student/portal/notes
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/student/portal/attendance
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/student/portal/results
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Students

### [GET] /api/admin/Students
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Students
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateStudentRequest](#createstudentrequest-schema)<br>**text/json**: [CreateStudentRequest](#createstudentrequest-schema)<br>**application/*+json**: [CreateStudentRequest](#createstudentrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/admin/Students/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [PUT] /api/admin/Students/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [UpdateStudentRequest](#updatestudentrequest-schema)<br>**text/json**: [UpdateStudentRequest](#updatestudentrequest-schema)<br>**application/*+json**: [UpdateStudentRequest](#updatestudentrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/admin/Students/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Students/import
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/admin/Students/export
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Subjects

### [GET] /api/admin/Subjects
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Subjects
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateSubjectRequest](#createsubjectrequest-schema)<br>**text/json**: [CreateSubjectRequest](#createsubjectrequest-schema)<br>**application/*+json**: [CreateSubjectRequest](#createsubjectrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [GET] /api/admin/Subjects/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [PUT] /api/admin/Subjects/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [UpdateSubjectRequest](#updatesubjectrequest-schema)<br>**text/json**: [UpdateSubjectRequest](#updatesubjectrequest-schema)<br>**application/*+json**: [UpdateSubjectRequest](#updatesubjectrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/admin/Subjects/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## TeacherAttendance

### [GET] /api/teacher/TeacherAttendance/sessions
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/teacher/TeacherAttendance/sessions
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateAttendanceSessionRequest](#createattendancesessionrequest-schema)<br>**text/json**: [CreateAttendanceSessionRequest](#createattendancesessionrequest-schema)<br>**application/*+json**: [CreateAttendanceSessionRequest](#createattendancesessionrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/teacher/TeacherAttendance/sessions/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## TeacherNotes

### [GET] /api/teacher/notes
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/teacher/notes
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateNoteRequest](#createnoterequest-schema)<br>**text/json**: [CreateNoteRequest](#createnoterequest-schema)<br>**application/*+json**: [CreateNoteRequest](#createnoterequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/teacher/notes/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## TeacherTests

### [GET] /api/teacher/tests
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/teacher/tests
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateTestRequest](#createtestrequest-schema)<br>**text/json**: [CreateTestRequest](#createtestrequest-schema)<br>**application/*+json**: [CreateTestRequest](#createtestrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/teacher/tests/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
## Vacancies

### [GET] /api/admin/Vacancies
#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [POST] /api/admin/Vacancies
#### Request Body

| Required | Schema |
| -------- | ------ |
|  No | **application/json**: [CreateVacancyRequest](#createvacancyrequest-schema)<br>**text/json**: [CreateVacancyRequest](#createvacancyrequest-schema)<br>**application/*+json**: [CreateVacancyRequest](#createvacancyrequest-schema)<br> |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

### [DELETE] /api/admin/Vacancies/{id}
#### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ------ |
| id | path |  | Yes | string (uuid) |

#### Responses

| Code | Description |
| ---- | ----------- |
| 200 | OK |

---
### Schemas

#### CreateAttendanceSessionRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| batchId | string (uuid) |  | No |
| attendanceDate | dateTime |  | No |
| takenByUserId | string (uuid) |  | No |

#### CreateBatchRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| name | string or null |  | No |
| courseId | string (uuid) |  | No |
| teacherUserId | string (uuid) or null |  | No |
| defaultFee | double or null |  | No |

#### CreateCourseRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| name | string or null |  | No |
| description | string or null |  | No |

#### CreateEnquiryRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| fullName | string or null |  | No |
| mobile | string or null |  | No |
| email | string or null |  | No |
| previousSchoolOrCollege | string or null |  | No |
| source | string or null |  | No |
| interestedCourseId | string (uuid) or null |  | No |
| assignedToUserId | string (uuid) or null |  | No |

#### CreateFeePlanRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| studentId | string (uuid) |  | No |
| courseId | string (uuid) |  | No |
| batchId | string (uuid) |  | No |
| totalFee | double |  | No |
| discountAmount | double |  | No |
| planType | string or null |  | No |

#### CreateNoteRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| title | string or null |  | No |
| description | string or null |  | No |
| filePath | string or null |  | No |
| originalFileName | string or null |  | No |
| storedFileName | string or null |  | No |
| fileType | string or null |  | No |
| courseId | string (uuid) |  | No |
| batchId | string (uuid) |  | No |
| subjectId | string (uuid) |  | No |
| uploadedByUserId | string (uuid) |  | No |

#### CreateNoticeRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| title | string or null |  | No |
| message | string or null |  | No |
| courseId | string (uuid) or null |  | No |
| batchId | string (uuid) or null |  | No |

#### CreateStudentRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| studentCode | string or null |  | No |
| fullName | string or null |  | No |
| mobile | string or null |  | No |
| email | string or null |  | No |
| dateOfBirth | dateTime or null |  | No |
| admissionDate | dateTime |  | No |

#### CreateSubjectRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| name | string or null |  | No |
| courseId | string (uuid) |  | No |

#### CreateTestRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| testName | string or null |  | No |
| testDate | dateTime |  | No |
| maxMarks | double |  | No |
| courseId | string (uuid) |  | No |
| batchId | string (uuid) |  | No |
| subjectId | string (uuid) |  | No |

#### CreateUserRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| fullName | string |  | Yes |
| email | string (email) |  | Yes |
| mobile | string or null |  | No |
| password | string |  | Yes |
| roleCode | string |  | Yes |

#### CreateVacancyRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| title | string or null |  | No |
| examCategory | string or null |  | No |
| lastDate | dateTime |  | No |

#### LoginRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| email | string or null |  | No |
| password | string or null |  | No |

#### RegisterInstituteRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| instituteName | string or null |  | No |
| ownerName | string or null |  | No |
| email | string or null |  | No |
| mobile | string or null |  | No |
| password | string or null |  | No |

#### UpdateBatchRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| name | string or null |  | No |
| courseId | string (uuid) |  | No |
| teacherUserId | string (uuid) or null |  | No |
| defaultFee | double or null |  | No |

#### UpdateCourseRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| name | string or null |  | No |
| description | string or null |  | No |
| isActive | boolean |  | No |

#### UpdateEnquiryRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| fullName | string or null |  | No |
| mobile | string or null |  | No |
| email | string or null |  | No |
| previousSchoolOrCollege | string or null |  | No |
| source | string or null |  | No |
| status | string or null |  | No |
| interestedCourseId | string (uuid) or null |  | No |
| assignedToUserId | string (uuid) or null |  | No |

#### UpdateStudentRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| fullName | string or null |  | No |
| mobile | string or null |  | No |
| email | string or null |  | No |
| dateOfBirth | dateTime or null |  | No |

#### UpdateSubjectRequest Schema

| Name | Type | Description | Required |
| ---- | ---- | ----------- | -------- |
| name | string or null |  | No |
| courseId | string (uuid) |  | No |
