
import request from '@/utils/request'

// Plane管理
export const PlaneQueryPage = params => request.get('/api/Plane/queryPage', { params })
export const PlaneFind = data => request.post('/api/Plane/find', data)
export const PlaneRemove = data => request.post('/api/Plane/remove', data)
export const PlaneSave = data => request.post('/api/Plane/save', data)
// PlaneType管理
export const PlaneTypeQueryPage = params => request.get('/api/PlaneType/queryPage', { params })
export const PlaneTypeFind = data => request.post('/api/PlaneType/find', data)
export const PlaneTypeRemove = data => request.post('/api/PlaneType/remove', data)
export const PlaneTypeSave = data => request.post('/api/PlaneType/save', data)

