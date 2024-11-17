import ActivityRelationship from '../activityRelationship'

export default interface GetActivitiesRequest {
  pageNumber: number
  pageSize: number
  fromDate?: Date
  toDate?: Date
  relationship: ActivityRelationship
}