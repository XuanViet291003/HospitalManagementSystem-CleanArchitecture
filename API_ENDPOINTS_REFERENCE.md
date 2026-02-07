# 📚 API ENDPOINTS REFERENCE - QUICK GUIDE

## 🔐 AUTHENTICATION

### Register Patient
```
POST /api/users/register
Body: { email, password, fullname, roleName: "Patient" }
Response: { userId }
```

### Login
```
POST /api/users/login
Body: { email, password }
Response: { token }
```

### Get Profile
```
GET /api/users/profile
Headers: Authorization: Bearer {token}
Response: { userId, email }
```

---

## 🏥 DEPARTMENTS

### Get All Departments
```
GET /api/departments
Response: [{ id, name, description }]
```

### Create Department (Admin)
```
POST /api/departments
Headers: Authorization: Bearer {token}, Role: Admin
Body: { name, description }
Response: { departmentId }
```

### Update Department (Admin)
```
PUT /api/departments/{id}
Body: { name, description }
```

### Delete Department (Admin)
```
DELETE /api/departments/{id}
```

---

## 👨‍⚕️ DOCTORS

### Get Doctors by Department
```
GET /api/doctors/department/{departmentId}
Response: [{ id, fullName, specialization, consultationFee, departmentName }]
```

---

## 📅 APPOINTMENTS

### Get Available Time Slots
```
GET /api/appointments/doctor/{doctorId}/available-slots?date=2024-12-15&slotDurationMinutes=30
Response: [{ startTime, endTime, isAvailable }]
```

### Create Appointment
```
POST /api/appointments
Headers: Authorization: Bearer {token}
Body: { doctorId, appointmentTime, durationMinutes, notes }
Response: { appointmentId }
```

### Get Patient Appointments
```
GET /api/appointments/patient/{patientId}
Headers: Authorization: Bearer {token}
Response: [{ id, appointmentTime, status, doctor, patient }]
```

### Check-in Appointment
```
POST /api/appointments/{id}/check-in
Headers: Authorization: Bearer {token}, Role: Admin, Receptionist
```

### Complete Appointment
```
POST /api/appointments/{id}/complete
Headers: Authorization: Bearer {token}, Role: Doctor, Admin
```

### Cancel Appointment
```
POST /api/appointments/{id}/cancel
Headers: Authorization: Bearer {token}
Body: { reason? }
```

---

## 📋 MEDICAL RECORDS

### Create Medical Record
```
POST /api/medicalrecords
Headers: Authorization: Bearer {token}, Role: Doctor, Admin
Body: { appointmentId, symptoms, diagnosis, treatmentPlan, followUpDate }
Response: { medicalRecordId }
```

### Get Medical Record by ID
```
GET /api/medicalrecords/{id}
Headers: Authorization: Bearer {token}
Response: { id, appointmentTime, symptoms, diagnosis, doctor, patient }
```

### Get Medical Records by Patient
```
GET /api/medicalrecords/patient/{patientId}
Headers: Authorization: Bearer {token}
Response: [{ ... }]
```

### Update Medical Record
```
PUT /api/medicalrecords/{id}
Headers: Authorization: Bearer {token}, Role: Doctor, Admin
Body: { symptoms?, diagnosis?, treatmentPlan?, followUpDate? }
```

---

## 💊 MEDICINES

### Get Medicines (Search)
```
GET /api/medicines?searchTerm=paracetamol
Response: [{ id, name, unit, stock, unitPrice }]
```

---

## 📝 PRESCRIPTIONS

### Create Prescription
```
POST /api/prescriptions
Headers: Authorization: Bearer {token}, Role: Doctor, Admin
Body: {
  medicalRecordId,
  items: [{ medicineId, quantity, dosage, notes }]
}
Response: { prescriptionId }
```

### Get Prescription by ID
```
GET /api/prescriptions/{id}
Headers: Authorization: Bearer {token}
Response: { id, issuedDate, totalAmount, items: [...] }
```

---

## 💰 INVOICES

### Create Invoice
```
POST /api/invoices
Headers: Authorization: Bearer {token}, Role: Admin, Receptionist
Body: {
  appointmentId,
  items?: [{ itemDescription, itemType, quantity, unitPrice }],
  dueDate?
}
Response: { invoiceId }
Note: Auto-adds consultation fee if appointment = Completed
```

### Get Invoice by ID
```
GET /api/invoices/{id}
Headers: Authorization: Bearer {token}
Response: { id, invoiceCode, totalAmount, status, items, patient }
```

### Get Invoices by Patient
```
GET /api/invoices/patient/{patientId}
Headers: Authorization: Bearer {token}
Response: [{ ... }]
```

### Add Invoice Item
```
POST /api/invoices/{invoiceId}/items
Headers: Authorization: Bearer {token}, Role: Admin, Receptionist
Body: { itemDescription, itemType, quantity, unitPrice }
Response: { invoiceItemId }
```

---

## 💳 PAYMENTS

### Create Payment
```
POST /api/payments
Headers: Authorization: Bearer {token}, Role: Admin, Receptionist
Body: {
  invoiceId,
  amount,
  paymentMethod: "Cash" | "Card" | "VNPay",
  transactionCode?
}
Response: { paymentId }
Note: Auto-updates Invoice status = Paid if fully paid
```

### Get Payments by Invoice
```
GET /api/payments/invoice/{invoiceId}
Headers: Authorization: Bearer {token}
Response: [{ id, amount, paymentMethod, paymentDate, invoiceCode }]
```

### Get Revenue Report (Admin)
```
GET /api/payments/revenue?startDate=2024-12-01&endDate=2024-12-31
Headers: Authorization: Bearer {token}, Role: Admin
Response: {
  totalRevenue,
  totalPayments,
  cashRevenue,
  cardRevenue,
  vnPayRevenue,
  dailyRevenues: [{ date, amount, paymentCount }]
}
```

---

## ⚙️ SYSTEM CONFIGURATIONS

### Get All Configurations (Admin)
```
GET /api/systemconfigurations
Headers: Authorization: Bearer {token}, Role: Admin
Response: [{ key, value, description }]
```

### Get Configuration by Key
```
GET /api/systemconfigurations/{key}
Response: { key, value, description }
```

### Update Configuration (Admin)
```
PUT /api/systemconfigurations/{key}
Headers: Authorization: Bearer {token}, Role: Admin
Body: { value, description? }
```

---

## 📊 STATUS CODES

### Appointment Status
- `0` = Scheduled (Đã đặt)
- `1` = Completed (Đã hoàn thành)
- `2` = Cancelled (Đã hủy)
- `3` = CheckedIn (Đã check-in)
- `4` = NoShow (Không đến)

### Invoice Status
- `0` = Unpaid (Chưa thanh toán)
- `1` = Paid (Đã thanh toán)
- `2` = Cancelled (Đã hủy)

### Payment Methods
- `"Cash"` = Tiền mặt
- `"Card"` = Thẻ
- `"VNPay"` = VNPay

### Item Types
- `"Consultation"` = Phí khám
- `"Medicine"` = Thuốc
- `"LabTest"` = Xét nghiệm

---

## 🔑 ROLES & PERMISSIONS

| Role | Permissions |
|------|-------------|
| **Patient** | Đặt lịch, xem lịch của mình, xem bệnh án, xem hóa đơn |
| **Doctor** | Complete appointment, tạo/cập nhật medical record, tạo prescription |
| **Admin** | Tất cả quyền |
| **Receptionist** | Check-in appointment, tạo invoice, thanh toán |

---

## 🎯 QUICK TEST SCENARIOS

### Scenario 1: Patient Journey
```
1. POST /api/users/register (role: Patient)
2. POST /api/users/login
3. GET /api/departments
4. GET /api/doctors/department/1
5. GET /api/appointments/doctor/1/available-slots?date=2024-12-15
6. POST /api/appointments
7. GET /api/appointments/patient/{patientId}
```

### Scenario 2: Doctor Workflow
```
1. POST /api/appointments/{id}/check-in
2. POST /api/appointments/{id}/complete
3. POST /api/medicalrecords
4. POST /api/prescriptions
```

### Scenario 3: Receptionist Workflow
```
1. POST /api/invoices
2. POST /api/payments
3. GET /api/payments/invoice/{invoiceId}
```

### Scenario 4: Admin Reports
```
1. GET /api/payments/revenue?startDate=2024-12-01&endDate=2024-12-31
2. GET /api/systemconfigurations
3. PUT /api/systemconfigurations/{key}
```


