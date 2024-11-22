import { makeAutoObservable, reaction, runInAction } from 'mobx'
import agent from '../api/agent'
import { format } from 'date-fns'
import PagingParams from '../models/pagingParams'
import PageParams from '../models/pageParams'
import { Activity } from '../models/activity'
import ActivityFormValues from '../models/forms/activityFormValues'
import ActivityDto from '../models/dtos/activityDto'
import { globalStore } from './globalStore'
import { ActivityListFilters } from '../models/activityListFilters'
import { GetActivitiesRequest } from '../models/requests/getActivitiesRequest'
import { ProfileShort } from '../models/profileShort'

export default class ActivityStore {
  updating: boolean = false
  loading: boolean = false
  isDirty: boolean = true

  activityRegistry: Map<number, Activity> = new Map<number, Activity>()
  private pageParams: PageParams | null = null
  selectedActivity: Activity | undefined = undefined

  listFilters: ActivityListFilters = new ActivityListFilters()

  constructor() {
    makeAutoObservable(this)

    reaction(
      () => ({
        fromDate: this.listFilters.fromDate,
        relationship: this.listFilters.relationship,
      }),
      this.handleFiltersChange
    )
  }

  get hasMore() {
    if (!this.pageParams) {
      return true
    } else {
      return this.pageParams.currentPage < this.pageParams.totalPages
    }
  }

  get groupedActivities() {
    return Object.entries(
      this.activitiesByDate.reduce((activities, activity) => {
        const date = format(activity.date!, 'dd MMM yyyy')
        activities[date] = activities[date]
          ? [...activities[date], activity]
          : [activity]
        return activities
      }, {} as { [key: string]: Activity[] })
    )
  }

  private get currentPage() {
    return this.pageParams?.currentPage ?? 0
  }

  private get activitiesByDate() {
    return Array.from(this.activityRegistry.values()).sort(
      (a, b) => a.date!.getTime() - b.date!.getTime()
    )
  }

  private setLoading = (value: boolean) => {
    this.loading = value
  }

  private setUpdating = (value: boolean) => {
    this.updating = value
  }

  private setDirty = (value: boolean) => {
    this.isDirty = value
  }

  private setPageParams = (pageParams: PageParams) => {
    this.pageParams = pageParams
  }

  private setSelectedActivity = (activity: Activity) => {
    this.selectedActivity = activity
  }

  setListFilter = <K extends keyof ActivityListFilters>(
    key: K,
    value: ActivityListFilters[K]
  ) => {
    this.listFilters[key] = value
  }

  loadNextActivitiesPage = async () => {
    this.setLoading(true)

    if (this.isDirty) {
      this.reset()
    }

    try {
      const pagingParams = new PagingParams(this.currentPage + 1)
      const request = new GetActivitiesRequest(pagingParams, this.listFilters)
      const pagedActivityList = await agent.Activities.list(request)

      pagedActivityList.items.forEach((a) => {
        this.addActivityInternal(a)
      })
      this.setPageParams(pagedActivityList.params)
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoading(false)
    }
  }

  loadActivity = async (id: number) => {
    let activity = this.getActivityInternal(id)

    if (!activity) {
      this.setLoading(true)

      try {
        const activityDto = await agent.Activities.details(id)
        activity = this.addActivityInternal(activityDto)
      } catch (error) {
        console.log(error)
      } finally {
        this.setLoading(false)
      }
    }

    if (activity) {
      this.setSelectedActivity(activity)
      this.setDirty(true)
    }
    return activity
  }

  clearSelectedActivity = () => {
    this.selectedActivity = undefined
  }

  createActivity = async (formValues: ActivityFormValues) => {
    try {
      const activityDto = await agent.Activities.create(formValues)
      const activity = this.addActivityInternal(activityDto)

      this.setSelectedActivity(activity)
      this.setDirty(true)

      return activity
    } catch (error) {
      console.log(error)
    }
  }

  updateActivity = async (formValues: ActivityFormValues) => {
    this.setUpdating(true)

    try {
      var activityDto = await agent.Activities.update(formValues)

      this.deleteActivityInternal(activityDto.id)
      const activity = this.addActivityInternal(activityDto)

      this.setSelectedActivity(activity)

      return activity
    } catch (error) {
      console.log(error)
    } finally {
      this.setUpdating(false)
    }
  }

  deleteActivity = async (id: number) => {
    this.setUpdating(true)

    try {
      await agent.Activities.delete(id)

      this.deleteActivityInternal(id)

      this.setDirty(true)
    } catch (error) {
      console.log(error)
    } finally {
      this.setUpdating(false)
    }
  }

  updateAttendance = async () => {
    const user = globalStore.userStore.user
    if (!this.selectedActivity || !user) {
      return
    }

    this.setUpdating(true)

    try {
      const isGoing = !this.selectedActivity.isGoing
      const id = this.selectedActivity.id
      await agent.Activities.updateAttendance(id, isGoing)

      runInAction(() => {
        const activity = this.getActivityInternal(id)
        if (!activity) {
          return
        }

        if (isGoing) {
          const attendee = new ProfileShort(user)
          activity.attendees.push(attendee)
        } else {
          activity.attendees = activity.attendees?.filter(
            (a) => a.username !== user.username
          )
        }
        activity.isGoing = isGoing
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setUpdating(false)
    }
  }

  cancelActivityToggle = async () => {
    const user = globalStore.userStore.user
    if (!this.selectedActivity || !user) {
      return
    }

    const formValues = new ActivityFormValues(this.selectedActivity)
    formValues.isCancelled = !formValues.isCancelled

    await this.updateActivity(formValues)
  }

  updateAttendeeFollowing = (username: string) => {
    this.activityRegistry.forEach((activity) => {
      activity.attendees?.forEach((attendee) => {
        if (attendee.username === username) {
          attendee.following
            ? attendee.followersCount--
            : attendee.followingCount++
          attendee.following = !attendee.following
        }
      })
    })
  }

  private reset = () => {
    this.activityRegistry.clear()
    this.pageParams = null
    this.selectedActivity = undefined
    this.setDirty(false)
  }

  private handleFiltersChange = () => {
    this.reset()
    this.loadNextActivitiesPage()
  }

  private getActivityInternal = (id: number) => {
    return this.activityRegistry.get(id)
  }

  private addActivityInternal = (activityDto: ActivityDto) => {
    const user = globalStore.userStore.user
    const activity = new Activity(activityDto, user)
    this.activityRegistry.set(activity.id, activity)

    return activity
  }

  private deleteActivityInternal = (id: number) => {
    this.activityRegistry.delete(id)
  }
}
