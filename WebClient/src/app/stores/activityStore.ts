import { makeAutoObservable, reaction, runInAction } from 'mobx'
import agent from '../api/agent'
import { format } from 'date-fns'
import { Profile } from '../models/profile'
import PagingParams from '../models/pagingParams'
import PageParams from '../models/pageParams'
import { Activity } from '../models/activity'
import ActivityFormValues from '../models/forms/activityFormValues'
import ActivityDto from '../models/dtos/activityDto'
import { globalStore } from './globalStore'
import { ActivityListFilters } from '../models/activityListFilters'
import { GetActivitiesRequest } from '../models/requests/getActivitiesRequest'

export default class ActivityStore {
  updating: boolean = false
  loading: boolean = false

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
      this.reset
    )
  }

  get hasMore() {
    return (
      (this.pageParams &&
        this.pageParams.currentPage < this.pageParams.totalPages) ||
      false
    )
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

  setListFilter = <K extends keyof ActivityListFilters>(
    key: K,
    value: ActivityListFilters[K]
  ) => {
    this.listFilters[key] = value
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

  loadNextActivitiesPage = async () => {
    this.setLoading(true)

    try {
      const pagingParams = new PagingParams(this.currentPage + 1)
      const request = new GetActivitiesRequest(pagingParams, this.listFilters)
      const result = await agent.Activities.list(request)

      runInAction(() => {
        result.items.forEach((a) => {
          this.addActivity(a)
        })
        this.pageParams = result.params
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setLoading(false)
    }
  }

  loadActivity = async (id: number) => {
    let activity = this.getActivity(id)

    if (!activity) {
      this.setLoading(true)

      try {
        const activityDto = await agent.Activities.details(id)
        activity = this.addActivity(activityDto)
      } catch (error) {
        console.log(error)
      } finally {
        this.setLoading(false)
      }
    }

    runInAction(() => (this.selectedActivity = activity))
    return activity
  }

  createActivity = async (formValues: ActivityFormValues) => {
    try {
      const activityDto = await agent.Activities.create(formValues)
      const activity = this.addActivity(activityDto)

      runInAction(() => {
        this.selectedActivity = activity
      })
    } catch (error) {
      console.log(error)
    }
  }

  updateActivity = async (formValues: ActivityFormValues) => {
    try {
      var activity = await agent.Activities.update(formValues)

      runInAction(() => {
        if (activity.id) {
          let updatedActivity = {
            ...this.getActivity(activity.id),
            ...activity,
          }
          this.activityRegistry.set(activity.id, updatedActivity as Activity)
          this.selectedActivity = updatedActivity as Activity
        }
      })
    } catch (error) {
      console.log(error)
    }
  }

  deleteActivity = async (id: number) => {
    this.setUpdating(true)

    try {
      await agent.Activities.delete(id)

      runInAction(() => {
        this.activityRegistry.delete(id)
      })
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
      const isGoing = this.selectedActivity.isGoing
      await agent.Activities.updateAttendance(
        this.selectedActivity.id,
        !isGoing
      )
      this.selectedActivity.isGoing = !isGoing

      runInAction(() => {
        if (this.selectedActivity?.isGoing) {
          this.selectedActivity.attendees =
            this.selectedActivity.attendees?.filter(
              (a) => a.username !== user.username
            )

          this.selectedActivity.isGoing = false
        } else {
          const attendee = new Profile(user)
          this.selectedActivity!.attendees?.push(attendee)
          this.selectedActivity!.isGoing = true
        }
        this.activityRegistry.set(
          this.selectedActivity!.id,
          this.selectedActivity!
        )
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

    this.setUpdating(true)

    try {
      const isGoing = this.selectedActivity.isGoing
      await agent.Activities.updateAttendance(
        this.selectedActivity!.id,
        !isGoing
      )

      runInAction(() => {
        this.selectedActivity!.isCancelled = !this.selectedActivity?.isCancelled
        this.activityRegistry.set(
          this.selectedActivity!.id,
          this.selectedActivity!
        )
      })
    } catch (error) {
      console.log(error)
    } finally {
      this.setUpdating(false)
    }
  }

  clearSelectedActivity = () => {
    this.selectedActivity = undefined
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
    this.loadNextActivitiesPage()
  }

  private getActivity = (id: number) => {
    return this.activityRegistry.get(id)
  }

  private addActivity = (activityDto: ActivityDto) => {
    const user = globalStore.userStore.user
    const activity = new Activity(activityDto, user)
    this.activityRegistry.set(activity.id, activity)

    return activity
  }
}
