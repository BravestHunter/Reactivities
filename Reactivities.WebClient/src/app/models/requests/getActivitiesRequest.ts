import { ActivityListFilters } from '../activityListFilters'
import ActivityRelationship from '../activityRelationship'
import PagingParams from '../pagingParams'

export interface GetActivitiesRequest {
  pageNumber: number
  pageSize: number
  fromDate?: Date
  toDate?: Date
  relationship: ActivityRelationship
}

export class GetActivitiesRequest implements GetActivitiesRequest {
  constructor(pagingParams: PagingParams, filters: ActivityListFilters) {
    this.pageNumber = pagingParams.pageNumber
    this.pageSize = pagingParams.pageSize
    this.fromDate = filters.fromDate
    this.relationship = filters.relationship
  }
}
